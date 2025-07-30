using AutoMapper;
using BankLoanSystem.Application.CQRS.Commands.Loan;
using BankLoanSystem.Application.Exceptions;
using BankLoanSystem.Application.Services;
using BankLoanSystem.Core.Interfaces.Repositories;
using BankLoanSystem.Core.Interfaces.Services;
using BankLoanSystem.Core.Models.DTOs.LoanDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Application.CQRS.Handlers.Loan
{
    public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, LoanDTO>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ILoanTypeRepository _loanTypeRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CreateLoanCommandHandler(ILoanRepository loanRepository, ILoanTypeRepository loanTypeRepository,
            IMapper mapper, IFileService fileService)
        {
            _loanRepository = loanRepository;
            _loanTypeRepository = loanTypeRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<LoanDTO> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
        {
            var loanType = await _loanTypeRepository.GetByIdAsync(request.LoanTypeId);
            if (loanType == null)
                throw new NotFoundException($"LoanType with ID {request.LoanTypeId} not found.");

            var loan = _mapper.Map<BankLoanSystem.Infrastructure.Loan>(request);

            loan.NationalIdPath = _fileService.UploadFile("NationalIds", request.NationalId) ?? "";

            loan.SalarySlipPath = _fileService.UploadFile("SalarySlips", request.SalarySlip) ?? "";

            var createdLoan = await _loanRepository.CreateAsync(loan);

            return _mapper.Map<LoanDTO>(createdLoan);
        }
    }
}
