using BankLoanSystem.Core.Models.DTOs.LoanTypeDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Application.CQRS.Commands.LoanType
{
    public record UpdateLoanTypeCommand : IRequest<LoanTypeDto?>
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }

}
