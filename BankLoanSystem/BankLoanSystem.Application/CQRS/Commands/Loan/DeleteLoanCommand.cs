using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Application.CQRS.Commands.Loan
{
    public record DeleteLoanCommand(int Id, string? CurrentUserId) : IRequest<bool>;

}
