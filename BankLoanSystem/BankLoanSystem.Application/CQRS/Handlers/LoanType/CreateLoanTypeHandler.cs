using AutoMapper;
using BankLoanSystem.Application.CQRS.Commands.LoanType;
using BankLoanSystem.Core.Interfaces.Repositories;
using BankLoanSystem.Core.Models.DTOs.LoanTypeDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankLoanSystem.Application.CQRS.Handlers.LoanType
{
    public class CreateLoanTypeHandler : IRequestHandler<CreateLoanTypeCommand, LoanTypeDto>
    {
        private readonly ILoanTypeRepository _repository;
        private readonly IMapper _mapper;

        public CreateLoanTypeHandler(ILoanTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<LoanTypeDto> Handle(CreateLoanTypeCommand request, CancellationToken cancellationToken)
        {
            // Check if name already exists
            var existingLoanType = await _repository.GetByNameAsync(request.Name);
            if (existingLoanType != null)
            {
                throw new InvalidOperationException("A loan type with the same name already exists.");
            }

            var loanType = _mapper.Map<BankLoanSystem.Infrastructure.LoanType>(request);

            var created = await _repository.CreateAsync(loanType);

            var loanTypeDto = _mapper.Map<LoanTypeDto>(created);

            return loanTypeDto;
        }

    }
}
