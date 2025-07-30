using AutoMapper;
using BankLoanSystem.Application.CQRS.Commands.Loan;
using BankLoanSystem.Core.Interfaces.Repositories;
using BankLoanSystem.Core.Interfaces.Services;
using BankLoanSystem.Core.Models.DTOs.EmailDtos;
using BankLoanSystem.Core.Models.DTOs.LoanDtos;
using BankLoanSystem.Core.Models.Entities;
using BankLoanSystem.Core.Models.Enums;
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
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public UpdateLoanStatusCommandHandler(ILoanRepository loanRepository, IEmailService emailService, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<LoanDTO?> Handle(UpdateLoanStatusCommand request, CancellationToken cancellationToken)
        {
            var existingLoan = await _loanRepository.GetByIdAsync((int) request.LoanId);
            if (existingLoan == null)
            {
                return null;
            }

            if (existingLoan.Status == request.LoanStatus)
            {
                return _mapper.Map<LoanDTO>(existingLoan);
            }

            existingLoan.Status = request.LoanStatus;

            var updatedLoan = await _loanRepository.UpdateAsync((int) request.LoanId, existingLoan);

            // Construct email body
            string emailBody = $@"
        Dear {updatedLoan?.AppUser?.Email},<br/>
        Your loan request (ID: <strong>{existingLoan.Id}</strong>) has been updated.<br/>
        The new status is: <strong>{((LoanStatus)request.LoanStatus).ToString()}</strong>.<br/>
        Please log in to your account for more details.<br/><br/>
        Thank you.
            ";

            EmailDto emailDto = new()
            {
                To = updatedLoan?.AppUser?.Email ?? "",
                Subject = "Loan Status Updated",
                Body = emailBody
            };

            bool isEmailSent = _emailService.SendEmail(emailDto);


            return _mapper.Map<LoanDTO>(updatedLoan);
        }
   
    }
}
