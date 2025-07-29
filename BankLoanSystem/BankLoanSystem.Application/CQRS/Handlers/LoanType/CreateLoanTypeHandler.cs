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
            var loanType = _mapper.Map<Core.Models.Entities.LoanType>(request);

            var created = await _repository.CreateAsync(loanType);

            var loanTypeDto = _mapper.Map<LoanTypeDto>(loanType);

            return loanTypeDto;
        }
    }
}
