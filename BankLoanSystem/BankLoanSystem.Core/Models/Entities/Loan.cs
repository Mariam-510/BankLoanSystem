using BankLoanSystem.Core.Models.Attributes;
using BankLoanSystem.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Core.Models.Entities
{
    public class Loan
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [NonNegative]
        public decimal Amount { get; set; }

        [Required]
        [NonNegative]
        public int Duration { get; set; }

        public LoanStatus Status { get; set; } = LoanStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
       
        [Required]
        public string NationalIdPath { get; set; } = string.Empty;

        [Required]
        public string SalarySlipPath { get; set; } = string.Empty;

        [ForeignKey("Client")]
        [Required]
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        [Required]
        [ForeignKey("LoanType")]
        public int LoanTypeId { get; set; }
        public LoanType? LoanType { get; set; }
    }

}
