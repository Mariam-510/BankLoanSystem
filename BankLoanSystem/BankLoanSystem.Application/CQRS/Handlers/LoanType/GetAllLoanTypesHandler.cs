using AutoMapper;
using BankLoanSystem.Application.CQRS.Queries.LoanType;
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
    public class GetAllLoanTypesHandler : IRequestHandler<GetAllLoanTypesQuery, List<LoanTypeDto>>
    {
        private readonly ILoanTypeRepository _repository;
        private readonly IMapper _mapper;

        public GetAllLoanTypesHandler(ILoanTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<LoanTypeDto>> Handle(GetAllLoanTypesQuery request, CancellationToken cancellationToken)
        {
            var loanTypes =  await _repository.GetAllAsync();
            var loanTypesDto = _mapper.Map<List<LoanTypeDto>>(loanTypes);

            return loanTypesDto;

        }
    }

}
