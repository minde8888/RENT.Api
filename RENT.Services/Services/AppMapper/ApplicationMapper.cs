using AutoMapper;
using RENT.Domain.Dtos;
using RENT.Domain.Dtos.Requests;
using RENT.Domain.Dtos.UpdateDto;
using RENT.Domain.Entities;
using RENT.Services.Services.Dtos;

namespace RENT.Services.Services.AppMapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Seller, UserRegistrationDto>().ReverseMap();
            CreateMap<Seller, UserInformationDto>().ReverseMap();

            CreateMap<Customers, UserRegistrationDto>().ReverseMap();
            CreateMap<Customers, UserInformationDto>().ReverseMap();
            //.ForMember(m => m.Address, opt =>
            //opt.MapFrom(m => m.Address));
            //CreateMap<Manager, SellerInformationDto>().ReverseMap();
            //CreateMap<Manager, ReturnManagerDto>().ReverseMap().
            //    ForMember(m => m.Address, opt =>
            //    opt.MapFrom(m => m.Address));

            //CreateMap<Employee, SellerInformationDto>().ReverseMap();
            //CreateMap<UserRegistrationDto, Employee>().ReverseMap();
            //CreateMap<RequestEmployeeDto, ReturnEmployeeDto>().ReverseMap()
            //    .ForMember(m => m.Address, opt =>
            //    opt.MapFrom(m => m.Address)); ;

            //CreateMap<AddressDto, Address>().ReverseMap();

            //CreateMap<Employee, EmployeeDto>().ReverseMap();

            //CreateMap<Post, PostDto>().ReverseMap();

            //CreateMap<Project, ProjectDto>().ReverseMap();

            //CreateMap<ProgressPlan, ProgressPlanDto>().ReverseMap();
            //CreateMap<ProgressPlan, ProgressPlanReturnDto>().ReverseMap();

            //CreateMap<Rent, RentDto>().ReverseMap();
        }
    }
}