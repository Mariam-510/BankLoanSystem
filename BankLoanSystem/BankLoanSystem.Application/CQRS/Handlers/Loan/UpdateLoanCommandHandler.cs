using AutoMapper;
using BankLoanSystem.Application.CQRS.Commands.Loan;
using BankLoanSystem.Core.Interfaces.Repositories;
using BankLoanSystem.Core.Interfaces.Services;
using BankLoanSystem.Core.Models.DTOs.LoanDtos;
using BankLoanSystem.Core.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Application.CQRS.Handlers.Loan
{
    public class UpdateLoanCommandHandler : IRequestHandler<UpdateLoanCommand, LoanDTO?>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UpdateLoanCommandHandler(ILoanRepository loanRepository, IMapper mapper, IFileService fileService)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<LoanDTO?> Handle(UpdateLoanCommand request, CancellationToken cancellationToken)
        {
            var existingLoan = await _loanRepository.GetByIdAsync((int) request.Id);
            if (existingLoan == null) return null;

            if (existingLoan.AppUserId != request.CurrentUserId)
            {
                return null;
            }

            if (existingLoan.Status != 0)
                return null;

            existingLoan.Duration = request.Duration;
            existingLoan.Amount = request.Amount;

            if(request.NationalId != null)
                existingLoan.NationalIdPath = _fileService.UpdateFile("NationalIds", request.NationalId, existingLoan.NationalIdPath);

            if (request.SalarySlip != null)
                existingLoan.SalarySlipPath = _fileService.UpdateFile("SalarySlips", request.SalarySlip, existingLoan.SalarySlipPath);


            var updatedLoan = await _loanRepository.UpdateAsync((int) request.Id, existingLoan);

            return _mapper.Map<LoanDTO>(updatedLoan);
        }



    }
}
