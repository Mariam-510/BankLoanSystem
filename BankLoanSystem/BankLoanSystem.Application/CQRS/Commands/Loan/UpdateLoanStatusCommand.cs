using BankLoanSystem.Core.Models.DTOs.LoanDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Application.CQRS.Commands.Loan
{
    public class UpdateLoanStatusCommand : IRequest<LoanDTO?>
    {
        public int LoanId { get; set; }
        public int LoanStatus { get; set; }

    }
}
