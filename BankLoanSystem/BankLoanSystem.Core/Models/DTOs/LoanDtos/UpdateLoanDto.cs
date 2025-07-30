using BankLoanSystem.Core.Models.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Core.Models.DTOs.LoanDtos
{
    public class UpdateLoanDto
    {
        [Required]
        [NonNegative("Amount must be non-negative")]
        public decimal Amount { get; set; }

        [Required]
        [NonNegative("Duration must be non-negative")]
        public int Duration { get; set; }

        [Required]
        [AllowedFileExtensions]
        public IFormFile NationalId { get; set; }

        [Required]
        [AllowedFileExtensions]
        public IFormFile SalarySlip { get; set; }

    }
}
