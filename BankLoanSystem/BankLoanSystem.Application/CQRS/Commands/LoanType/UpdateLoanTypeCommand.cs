using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Application.CQRS.Commands.LoanType
{
    public record UpdateLoanTypeCommand(int Id, string Name) : IRequest<bool>;

}
