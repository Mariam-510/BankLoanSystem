using AutoMapper;
using BankLoanSystem.Application.CQRS.Queries;
using BankLoanSystem.Core.Interfaces.Repositories;
using BankLoanSystem.Core.Models.DTOs.LoanTypeDtos;
using BankLoanSystem.Core.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Application.CQRS.Handlers.LoanType
{
    public class GetLoanTypeByIdHandler : IRequestHandler<GetLoanTypeByIdQuery, LoanTypeDto?>
    {
        private readonly ILoanTypeRepository _repository;
        private readonly IMapper _mapper;

        public GetLoanTypeByIdHandler(ILoanTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LoanTypeDto?> Handle(GetLoanTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var loanType = await _repository.GetByIdAsync(request.Id);
            if (loanType == null)
            {
                return null;
            }
            var loanTypeDto = _mapper.Map<LoanTypeDto>(loanType);

            return loanTypeDto;
        }
    }
}
