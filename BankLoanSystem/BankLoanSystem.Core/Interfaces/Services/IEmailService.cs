using BankLoanSystem.Core.Models.DTOs.EmailDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Core.Interfaces.Services
{
    public interface IEmailService
    {
        bool SendEmail(EmailDto request);
    }
}
