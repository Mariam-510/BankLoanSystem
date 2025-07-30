using BankLoanSystem.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Infrastructure.DbContext
{
    public static class DbInitializer
    {
        //----------------------------------------------------------------------------------------------------
        static string AdminRoleId = "266c6ce3-1f56-4601-9d45-5d9b87b01b30";
        static string ClientRoleId = "cdb95803-0507-4a46-9c14-0c3b3055c9be";
        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id=AdminRoleId,
                    ConcurrencyStamp=AdminRoleId,
                    Name="Admin",
                    NormalizedName="Admin".ToUpper(),
                },
                new IdentityRole
                {
                    Id=ClientRoleId,
                    ConcurrencyStamp=ClientRoleId,
                    Name="Client",
                    NormalizedName="Client".ToUpper(),
                }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }

        //----------------------------------------------------------------------------------------------------
        public static void SeedAdmins(ModelBuilder modelBuilder)
        {
            var appUserId = "779af355-c7c9-4935-ad2f-85c324825ff2";
            //Seed Account
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = appUserId,
                    FirstName = "Admin",
                    UserName = "admin1@gmail.com",
                    NormalizedUserName = "ADMIN1@GMAIL.COM",
                    Email = "admin1@gmail.com",
                    NormalizedEmail = "ADMIN1@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEG7/LUrcXKZaK3krqZoKCu3SYiqlupsTeHA4lnjWi0QONNvF0hF5zBNXYhZKBOwh/Q==", //Admin@123
                    SecurityStamp = "31012730-e125-4ecb-8811-38744d3a6160",
                    ConcurrencyStamp = "223d0eed-1b02-44c7-9bae-baae3508f127",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    CreatedAt = new DateTime(2025, 7, 29),
                });

            //Assign Role to Admin Account
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = appUserId,
                    RoleId = AdminRoleId
                });

        }
    }
 }
