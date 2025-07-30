using BankLoanSystem.Core.Models.DTOs.AppUserDtos;
using BankLoanSystem.Core.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> RegisterAsync(CreateDto createDto);
        Task<ApiResponse<object>> LoginAsync(LoginDto loginDto);
        Task<ApiResponse<object>> CreateAdminAsync(CreateDto createDto);
        Task<ApiResponse<object>> ConfirmEmailCodeAsync(string email, string code);
        Task<ApiResponse<object>> ResendConfirmationEmailAsync(EmailFormDto resendConfirmationEmailDto);
        Task<ApiResponse<object>> ForgotPasswordAsync(EmailFormDto emailFormDto);
        Task<ApiResponse<object>> ValidateResetCodeAsync(ValidateResetCodeDto validateResetCodeDto);
        Task<ApiResponse<object>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<ApiResponse<object>> DeleteAccountAsync(string userId);

    }
}
