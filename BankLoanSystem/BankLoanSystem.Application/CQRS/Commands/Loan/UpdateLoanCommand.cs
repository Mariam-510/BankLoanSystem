using BankLoanSystem.Core.Models.Attributes;
using BankLoanSystem.Core.Models.DTOs.LoanDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Application.CQRS.Commands.Loan
{
    public class UpdateLoanCommand : IRequest<LoanDTO?>
    {
        public int Id { get; set; }

        [Required]
        [NonNegative("Amount must be non-negative")]
        public decimal Amount { get; set; }

        [Required]
        [NonNegative("Duration must be non-negative")]
        public int Duration { get; set; }

        [AllowedFileExtensions]
        public IFormFile? NationalId { get; set; }

        [AllowedFileExtensions]
        public IFormFile? SalarySlip { get; set; }

        public string CurrentUserId { get; set; }

    }
}
