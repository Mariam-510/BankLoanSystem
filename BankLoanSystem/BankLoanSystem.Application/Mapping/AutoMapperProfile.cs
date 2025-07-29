using AutoMapper;
using BankLoanSystem.Core.Models.Entities;
using BankLoanSystem.Core.Models.DTOs.LoanTypeDtos;
using BankLoanSystem.Core.Models.DTOs.AppUserDtos;
using BankLoanSystem.Application.CQRS.Commands.LoanType;
using BankLoanSystem.Core.Models.DTOs.LoanDtos;
using BankLoanSystem.Core.Models.Enums;
using BankLoanSystem.Application.CQRS.Commands.Loan;


namespace BankLoanSystem.Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateDto, AppUser>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.EmailConfirmationCode, opt => opt.Ignore())
                .ForMember(dest => dest.CodeGeneratedAt, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordResetCode, opt => opt.Ignore())
                .ForMember(dest => dest.ResetCodeGeneratedAt, opt => opt.Ignore());
            
            //-----------------------------------------------------------------------------------
            CreateMap<LoanType, LoanTypeDto>().ReverseMap();

            CreateMap<CreateLoanTypeCommand, LoanType>().ReverseMap();

            //-----------------------------------------------------------------------------------
            CreateMap<CreateLoanCommand, Loan>()
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => LoanStatus.Pending))
                 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                 .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false));

            CreateMap<Loan, LoanDTO>()
                .ForMember(dest => dest.AppUserFirstName, opt => opt.MapFrom(src => src.AppUser != null ? src.AppUser.FirstName : null))
                .ForMember(dest => dest.AppUserLastName, opt => opt.MapFrom(src => src.AppUser != null ? src.AppUser.LastName : null))
                .ForMember(dest => dest.LoanTypeName, opt => opt.MapFrom(src => src.LoanType != null ? src.LoanType.Name : null));
        }

    }
}
