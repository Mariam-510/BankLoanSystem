using AutoMapper;
using BankLoanSystem.Application.Services;
using BankLoanSystem.Core.Interfaces.Services;
using BankLoanSystem.Core.Models.DTOs.AppUserDtos;
using BankLoanSystem.Core.Models.DTOs.EmailDtos;
using BankLoanSystem.Core.Models.DTOs.JWTDtos;
using BankLoanSystem.Core.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankLoanSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] CreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Invalid request data",
                    400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _authService.RegisterAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Invalid request data",
                    400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _authService.LoginAsync(loginDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("CreateAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAdmin([FromForm] CreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Invalid request data",
                    400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _authService.CreateAdminAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("ConfirmEmailCode")]
        public async Task<IActionResult> ConfirmEmailCode([FromBody] ConfirmEmailCodeFormDto confirmEmailCodeFormDto)
        {
            var result = await _authService.ConfirmEmailCodeAsync(confirmEmailCodeFormDto.Email, confirmEmailCodeFormDto.Code);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("ResendConfirmEmail")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] EmailFormDto resendConfirmationEmailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Invalid request data",
                    400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _authService.ResendConfirmationEmailAsync(resendConfirmationEmailDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailFormDto emailFormDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Invalid request data",
                    400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _authService.ForgotPasswordAsync(emailFormDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("ValidateResetCode")]
        public async Task<IActionResult> ValidateResetCode([FromBody] ValidateResetCodeDto validateResetCodeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Invalid request data",
                    400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _authService.ValidateResetCodeAsync(validateResetCodeDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Invalid request data",
                    400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _authService.ResetPasswordAsync(resetPasswordDto);
            return StatusCode(result.StatusCode, result);
        }
    }
}