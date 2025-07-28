using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Core.Models.Entities
{
    public class LoanType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<Loan>? Loans { get; set; }
    }
}
