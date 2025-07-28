using BankLoanSystem.Core.Interfaces.Repositories;
using BankLoanSystem.Core.Models.Attributes;
using BankLoanSystem.Core.Models.Entities;
using BankLoanSystem.Infrastructure.DbContext;
using BankLoanSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<LoanDbContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("BankLoanConnection"))
            );

            services.AddIdentityCore<AppUser>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<AppUser>>("Bank Loan System")
                .AddEntityFrameworkStores<LoanDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    NameClaimType = ClaimTypes.NameIdentifier
                };

            });


            // Add Scoped for repositories
            services.AddScoped<IPasswordValidator<AppUser>, PasswordLengthValidator<AppUser>>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<ILoanTypeRepository, LoanTypeRepository>();


            return services;
        }
    }
}
