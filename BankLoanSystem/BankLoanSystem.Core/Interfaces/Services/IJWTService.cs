using BankLoanSystem.Core.Models.DTOs.JWTDtos;
using BankLoanSystem.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Core.Interfaces.Services
{
    public interface IJWTService
    {
        string CreateJWTToken(Account appUser, List<string> roles, UserClaimsDto userData);
    }
}
