using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;    
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Core.Models.DTOs.AppUserDtos
{
    public class EmailFormDto
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
    }
}
