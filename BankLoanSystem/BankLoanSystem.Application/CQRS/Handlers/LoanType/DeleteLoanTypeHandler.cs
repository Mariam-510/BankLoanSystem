using BankLoanSystem.Application.CQRS.Commands.LoanType;
using BankLoanSystem.Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Application.CQRS.Handlers.LoanType
{
    public class DeleteLoanTypeHandler : IRequestHandler<DeleteLoanTypeCommand, bool>
    {
        private readonly ILoanTypeRepository _repository;

        public DeleteLoanTypeHandler(ILoanTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteLoanTypeCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing == null) return false;

            await _repository.DeleteAsync(request.Id);
            return true;
        }
    }
}
