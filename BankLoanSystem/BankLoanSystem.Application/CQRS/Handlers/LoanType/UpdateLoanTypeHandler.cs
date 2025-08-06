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
    public class UpdateLoanTypeHandler : IRequestHandler<UpdateLoanTypeCommand, LoanTypeDto?>
    {
        private readonly ILoanTypeRepository _repository;
        private readonly IMapper _mapper;

        public UpdateLoanTypeHandler(ILoanTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LoanTypeDto?> Handle(UpdateLoanTypeCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync((int) request.Id);
            if (existing == null) return null;

            // Check if name already exists
            var existingLoanType = await _repository.GetByNameAsync(request.Name);
            if (existingLoanType != null && existingLoanType.Id != existing.Id)
            {
                throw new InvalidOperationException("A loan type with the same name already exists.");
            }

            existing.Name = request.Name;
            var loanType = await _repository.UpdateAsync((int) request.Id, existing);

            var loanTypeDto = _mapper.Map<LoanTypeDto>(loanType);

            return loanTypeDto;
        }
    }
}
