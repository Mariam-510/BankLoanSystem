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
    public class AuthDbContext : IdentityDbContext<AppUser>
    {

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            DbInitializer.SeedRoles(builder);

            DbInitializer.SeedAdmins(builder);
        }
    }
}

