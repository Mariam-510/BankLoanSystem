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
    public class GetLoansByUserQueryHandler : IRequestHandler<GetLoansByUserQuery, List<LoanDTO>>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public GetLoansByUserQueryHandler(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<List<LoanDTO>> Handle(GetLoansByUserQuery request, CancellationToken cancellationToken)
        {
            var loans = await _loanRepository.GetAllByUserAsync(request.AppUserId);

            return _mapper.Map<List<LoanDTO>>(loans);
        }
    }
}
