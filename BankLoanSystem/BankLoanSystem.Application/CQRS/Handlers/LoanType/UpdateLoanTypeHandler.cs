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
    public class UpdateLoanTypeHandler : IRequestHandler<UpdateLoanTypeCommand, bool>
    {
        private readonly ILoanTypeRepository _repository;

        public UpdateLoanTypeHandler(ILoanTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateLoanTypeCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing == null) return false;

            existing.Name = request.Name;
            await _repository.UpdateAsync(request.Id, existing);
            return true;
        }
    }
}
