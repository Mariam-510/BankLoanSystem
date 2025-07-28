using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Core.Models.Entities
{
    public class AppUser : IdentityUser
    {

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "First Name must contain only letters.")]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        [MinLength(1)]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Last Name must contain only letters.")]
        public string? LastName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
        public ICollection<Loan>? Loans { get; set; }

    }
}
