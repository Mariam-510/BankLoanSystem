using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Core.Models.Entities
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Name must contain only letters.")]
        public string Name { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("Account")]
        [Required]
        public string AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
