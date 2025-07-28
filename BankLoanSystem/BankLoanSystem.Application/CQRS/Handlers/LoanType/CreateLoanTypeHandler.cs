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
    public class CreateLoanTypeHandler : IRequestHandler<CreateLoanTypeCommand, int>
    {
        private readonly ILoanTypeRepository _repository;

        public CreateLoanTypeHandler(ILoanTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateLoanTypeCommand request, CancellationToken cancellationToken)
        {
            var loanType = new Core.Models.Entities.LoanType { Name = request.Name };
            var created = await _repository.CreateAsync(loanType);
            return created.Id;
        }
    }
}
