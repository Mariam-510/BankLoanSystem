using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Core.Models.DTOs.LoanTypeDtos
{
    public class UpdateLoanTypeDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
