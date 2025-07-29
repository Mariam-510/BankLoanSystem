using AutoMapper;
using BankLoanSystem.Core.Interfaces.Services;
using BankLoanSystem.Core.Models.DTOs.AppUserDtos;
using BankLoanSystem.Core.Models.DTOs.EmailDtos;
using BankLoanSystem.Core.Models.DTOs.JWTDtos;
using BankLoanSystem.Core.Models.Entities;
using BankLoanSystem.Core.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Transactions;

namespace BankLoanSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJWTService _jWTService;
        private readonly IEmailService _emailService;

        public AuthService(
            IMapper mapper,
            UserManager<AppUser> userManager,
            IJWTService jWTService,
            IEmailService emailService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jWTService = jWTService;
            _emailService = emailService;
        }

        public async Task<ApiResponse<object>> RegisterAsync(CreateDto createDto)
        {
            try
            {
                var existingUser = await _userManager.Users
                    .Where(u => u.Email.ToLower() == createDto.Email.ToLower() && !u.IsDeleted)
                    .FirstOrDefaultAsync();

                if (existingUser != null)
                {
                    return ApiResponse<object>.ErrorResponse("Email already exists.", 400);
                }

                var appUser = _mapper.Map<AppUser>(createDto);

                var registerResult = await _userManager.CreateAsync(appUser, createDto.Password);

                if (registerResult.Succeeded)
                {
                    registerResult = await _userManager.AddToRoleAsync(appUser, "Client");

                    if (!registerResult.Succeeded)
                    {
                        return ApiResponse<object>.ErrorResponse(
                            "Failed to assign role.",
                            400,
                            registerResult.Errors.Select(e => e.Description).ToList());
                    }

                    var confirmationCode = new Random().Next(100000, 999999).ToString();
                    appUser.EmailConfirmationCode = confirmationCode;
                    appUser.CodeGeneratedAt = DateTime.UtcNow;
                    await _userManager.UpdateAsync(appUser);

                    string emailBody = $@"
                            Dear {appUser.Email},<br/>
                            Thank you for registering.<br/>
                            Your email confirmation code is: <strong>{confirmationCode}</strong><br/>
                            This code will expire in 2 minutes.<br/>
                            Please enter this code in the app to confirm your email.";

                    EmailDto emailDto = new()
                    {
                        To = appUser.Email,
                        Subject = "Email Confirmation",
                        Body = emailBody
                    };

                    bool isEmailSent = _emailService.SendEmail(emailDto);

                    if (!isEmailSent)
                    {
                        return ApiResponse<object>.ErrorResponse(
                            "Failed to send confirmation email. Please try again.",
                            500);
                    }

                    return ApiResponse<object>.SuccessResponse(
                        null,
                        "Registered Successfully! Check your email to confirm your account.",
                        201);
                }

                return ApiResponse<object>.ErrorResponse(
                    "Registration failed",
                    400,
                    registerResult.Errors.Select(e => e.Description).ToList());
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResponse(
                    "An error occurred while processing your request.",
                    500);
            }
        }

        public async Task<ApiResponse<object>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Where(u => u.Email.ToLower() == loginDto.Email.ToLower() && !u.IsDeleted)
                .FirstOrDefaultAsync();

            if (user == null || user.IsDeleted)
            {
                return ApiResponse<object>.ErrorResponse(
                    "UserName or Password Incorrect",
                    401);
            }

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
            {
                return ApiResponse<object>.ErrorResponse(
                    "Please confirm your email before logging in.",
                    403);
            }


            var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (checkPasswordResult)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var jwtToken = _jWTService.CreateJWTToken(user, roles.ToList());

                return ApiResponse<object>.SuccessResponse(
                    new JWTTokenDto { JwtToken = jwtToken },
                    "Login successful");
            }

            return ApiResponse<object>.ErrorResponse(
                "UserName or Password Incorrect",
                401);
        }

        public async Task<ApiResponse<object>> CreateAdminAsync(CreateDto createDto)
        {
            try
            {
                var existingUser = await _userManager.Users
                    .Where(u => u.Email.ToLower() == createDto.Email.ToLower() && !u.IsDeleted)
                    .FirstOrDefaultAsync();

                if (existingUser != null)
                {
                    return ApiResponse<object>.ErrorResponse("Email already exists.", 400);
                }

                var appUser = _mapper.Map<AppUser>(createDto);
                appUser.EmailConfirmed = true;

                var registerResult = await _userManager.CreateAsync(appUser, createDto.Password);

                if (registerResult.Succeeded)
                {
                    registerResult = await _userManager.AddToRoleAsync(appUser, "Admin");

                    if (!registerResult.Succeeded)
                    {
                        return ApiResponse<object>.ErrorResponse(
                            "Failed to assign role.",
                            400,
                            registerResult.Errors.Select(e => e.Description).ToList());
                    }

                    return ApiResponse<object>.SuccessResponse(
                        null,
                        "Admin Created Successfully!",
                        201);
                }

                return ApiResponse<object>.ErrorResponse(
                    "Registration failed",
                    400,
                    registerResult.Errors.Select(e => e.Description).ToList());
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResponse(
                    "An error occurred while processing your request.",
                    500);
            }
        }

        public async Task<ApiResponse<object>> ConfirmEmailCodeAsync(string email, string code)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
                return ApiResponse<object>.ErrorResponse("Email and code are required.", 400);

            var user = await _userManager.Users
                .Where(u => u.Email.ToLower() == email.ToLower() && !u.IsDeleted)
                .FirstOrDefaultAsync();

            if (user == null)
                return ApiResponse<object>.ErrorResponse("User not found.", 404);

            if (user.EmailConfirmed)
                return ApiResponse<object>.ErrorResponse("Email is already confirmed.", 409);

            if (user.CodeGeneratedAt.HasValue && (DateTime.UtcNow - user.CodeGeneratedAt.Value).TotalMinutes > 2)
                return ApiResponse<object>.ErrorResponse("The confirmation code has expired.", 400);

            if (user.EmailConfirmationCode != code)
                return ApiResponse<object>.ErrorResponse("Invalid confirmation code.", 400);

            user.EmailConfirmed = true;
            user.EmailConfirmationCode = null;
            user.CodeGeneratedAt = null;
            await _userManager.UpdateAsync(user);

            return ApiResponse<object>.SuccessResponse(null, "Email confirmed successfully!");
        }

        public async Task<ApiResponse<object>> ResendConfirmationEmailAsync(EmailFormDto resendConfirmationEmailDto)
        {
            try
            {
                var user = await _userManager.Users
                    .Where(u => u.Email.ToLower() == resendConfirmationEmailDto.Email.ToLower() && !u.IsDeleted)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return ApiResponse<object>.ErrorResponse("Email not found.", 404);
                }

                if (user.EmailConfirmed)
                {
                    return ApiResponse<object>.ErrorResponse("Email is already confirmed.", 409);
                }

                var confirmationCode = new Random().Next(100000, 999999).ToString();
                user.EmailConfirmationCode = confirmationCode;
                user.CodeGeneratedAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                string emailBody = $@"
                        Dear {user.Email},<br/>
                        Thank you for registering.<br/>
                        Your email confirmation code is: <strong>{confirmationCode}</strong><br/>
                        This code will expire in 2 minutes.<br/>
                        Please enter this code in the app to confirm your email.";

                EmailDto emailDto = new()
                {
                    To = user.Email,
                    Subject = "Email Confirmation",
                    Body = emailBody
                };

                bool isEmailSent = _emailService.SendEmail(emailDto);

                if (!isEmailSent)
                {
                    return ApiResponse<object>.ErrorResponse("Failed to send confirmation email. Please try again.", 500);
                }

                return ApiResponse<object>.SuccessResponse(null, "Check your email to confirm your account.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResponse("Failed to send confirmation email. Please try again.", 500);
            }
        }

        public async Task<ApiResponse<object>> ForgotPasswordAsync(EmailFormDto emailFormDto)
        {
            var user = await _userManager.Users
                    .Where(u => u.Email.ToLower() == emailFormDto.Email.ToLower() && !u.IsDeleted)
                    .FirstOrDefaultAsync();

            if (user == null)
                return ApiResponse<object>.ErrorResponse("Email doesn't exist.", 404);

            if (!user.EmailConfirmed)
                return ApiResponse<object>.ErrorResponse("Please confirm your email first.", 403);

            var resetCode = new Random().Next(100000, 999999).ToString();
            user.PasswordResetCode = resetCode;
            user.ResetCodeGeneratedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var emailBody = $@"
                    <p>Your password reset code is: <strong>{resetCode}</strong></p>
                    <p>This code will expire in 2 minutes.</p>";

            EmailDto emailDto = new()
            {
                To = user.Email,
                Subject = "Password Reset Code",
                Body = emailBody
            };

            var isEmailSent = _emailService.SendEmail(emailDto);
            if (!isEmailSent)
                return ApiResponse<object>.ErrorResponse("Failed to send reset code.", 500);

            return ApiResponse<object>.SuccessResponse(null, "Password reset code sent successfully.");
        }

        public async Task<ApiResponse<object>> ValidateResetCodeAsync(ValidateResetCodeDto validateResetCodeDto)
        {
            var user = await _userManager.Users
                .Where(u => u.Email.ToLower() == validateResetCodeDto.Email.ToLower() && !u.IsDeleted)
                .FirstOrDefaultAsync();

            if (user == null)
                return ApiResponse<object>.ErrorResponse("User not found.", 404);

            if (user.PasswordResetCode != validateResetCodeDto.Code)
                return ApiResponse<object>.ErrorResponse("Invalid reset code.", 400);

            if (user.ResetCodeGeneratedAt.HasValue &&
                (DateTime.UtcNow - user.ResetCodeGeneratedAt.Value).TotalMinutes > 2)
            {
                return ApiResponse<object>.ErrorResponse("Reset code has expired.", 400);
            }

            return ApiResponse<object>.SuccessResponse(null, "Code is valid.");
        }

        public async Task<ApiResponse<object>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.Users
                .Where(u => u.Email.ToLower() == resetPasswordDto.Email.ToLower() && !u.IsDeleted)
                .FirstOrDefaultAsync();

            if (user == null)
                return ApiResponse<object>.ErrorResponse("User not found.", 404);

            if (user.PasswordResetCode == null || user.ResetCodeGeneratedAt == null)
            {
                return ApiResponse<object>.ErrorResponse("Reset code is invalid.", 400);
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPasswordDto.NewPassword);

            if (result.Succeeded)
            {
                user.PasswordResetCode = null;
                user.ResetCodeGeneratedAt = null;
                await _userManager.UpdateAsync(user);

                return ApiResponse<object>.SuccessResponse(null, "Password reset successful.");
            }

            return ApiResponse<object>.ErrorResponse(
                "Password reset failed",
                400,
                result.Errors.Select(e => e.Description).ToList());
        }
    
    
    }

}