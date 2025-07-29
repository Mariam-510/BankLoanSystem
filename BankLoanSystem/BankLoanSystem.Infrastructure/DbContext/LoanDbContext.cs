    using BankLoanSystem.Core.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Infrastructure.DbContext
{
    public class LoanDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }

        public LoanDbContext(DbContextOptions<LoanDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //SeedRoles
            DbInitializer.SeedRoles(builder);
        }
    }
}

