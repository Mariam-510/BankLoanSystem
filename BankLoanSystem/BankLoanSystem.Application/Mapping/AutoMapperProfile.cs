using AutoMapper;
using BankLoanSystem.Core.Models.Entities;
using BankLoanSystem.Core.Models.DTOs.LoanTypeDtos;


namespace BankLoanSystem.Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LoanType, LoanTypeDto>().ReverseMap();
        }

    }
}
