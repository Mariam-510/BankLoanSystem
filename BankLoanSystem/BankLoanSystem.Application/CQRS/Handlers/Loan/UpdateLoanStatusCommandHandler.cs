using AutoMapper;
using BankLoanSystem.Application.CQRS.Commands.Loan;
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
    public class UpdateLoanStatusCommandHandler : IRequestHandler<UpdateLoanStatusCommand, LoanDTO?>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public UpdateLoanStatusCommandHandler(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<LoanDTO?> Handle(UpdateLoanStatusCommand request, CancellationToken cancellationToken)
        {
            var existingLoan = await _loanRepository.GetByIdAsync((int) request.LoanId);
            if (existingLoan == null)
            {
                return null;
            }

            existingLoan.Status = request.LoanStatus;

            var updatedLoan = await _loanRepository.UpdateAsync((int) request.LoanId, existingLoan);

            return _mapper.Map<LoanDTO>(updatedLoan);
        }
    }
}
