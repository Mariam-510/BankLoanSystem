using BankLoanSystem.Core.Models.DTOs.LoanTypeDtos;
using BankLoanSystem.Core.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Application.CQRS.Queries.LoanType
{
    public record GetLoanTypeByIdQuery(int Id) : IRequest<LoanTypeDto?>;

}
