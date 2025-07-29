using BankLoanSystem.Application.CQRS.Commands.Loan;
using BankLoanSystem.Core.Interfaces.Repositories;
using BankLoanSystem.Core.Models.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Application.CQRS.Handlers.Loan
{
    public class DeleteLoanCommandHandler : IRequestHandler<DeleteLoanCommand, bool>
    {
        private readonly ILoanRepository _loanRepository;

        public DeleteLoanCommandHandler(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<bool> Handle(DeleteLoanCommand request, CancellationToken cancellationToken)
        {
            var loan = await _loanRepository.GetByIdAsync(request.Id);

            if (loan == null)
                return false;

            if (loan.Status != LoanStatus.Pending)
                return false;

            var result = await _loanRepository.DeleteAsync(request.Id);

            return result != null;
        }
    }
}
