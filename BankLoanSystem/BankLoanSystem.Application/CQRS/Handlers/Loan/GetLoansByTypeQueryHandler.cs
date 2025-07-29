using AutoMapper;
using BankLoanSystem.Application.CQRS.Queries.Loan;
using BankLoanSystem.Core.Interfaces.Repositories;
using BankLoanSystem.Core.Models.DTOs.LoanDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Application.CQRS.Handlers.Loan
{
    public class GetLoansByTypeQueryHandler : IRequestHandler<GetLoansByTypeQuery, List<LoanDTO>>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public GetLoansByTypeQueryHandler(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<List<LoanDTO>> Handle(GetLoansByTypeQuery request, CancellationToken cancellationToken)
        {
            var loans = await _loanRepository.GetAllByTypeAsync(request.LoanTypeId);
            return _mapper.Map<List<LoanDTO>>(loans);
        }
    }
}
