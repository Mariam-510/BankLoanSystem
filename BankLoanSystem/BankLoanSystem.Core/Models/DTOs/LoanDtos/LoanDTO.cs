using BankLoanSystem.Core.Models.Attributes;
using BankLoanSystem.Core.Models.Entities;
using BankLoanSystem.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Core.Models.DTOs.LoanDtos
{
    public class LoanDTO
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public int Duration { get; set; }

        public LoanStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public string NationalIdPath { get; set; }

        public string SalarySlipPath { get; set; }

        public string AppUserId { get; set; }
        
        public string AppUserFirstName { get; set; }
        
        public string? AppUserLastName { get; set; }

        public int LoanTypeId { get; set; }

        public string LoanTypeName { get; set; }
    }
}
