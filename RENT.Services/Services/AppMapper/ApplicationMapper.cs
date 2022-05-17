using AutoMapper;
using RENT.Domain.Dtos;
using RENT.Domain.Entities;

namespace RENT.Services.Services.AppMapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Seller, UserRegistrationDto>().ReverseMap();
            CreateMap<Seller, UserInformationDto>().ReverseMap();
            CreateMap<Seller, UserDto>().ReverseMap().
               ForMember(m => m.Address, opt =>
               opt.MapFrom(m => m.Address));
            CreateMap<Seller, UserDto>().ReverseMap().
               ForMember(m => m.Address, opt =>
               opt.MapFrom(m => m.Address));

            CreateMap<Customers, UserRegistrationDto>().ReverseMap();
            CreateMap<Customers, UserInformationDto>().ReverseMap();
            CreateMap<Customers, UserDto>().ReverseMap().
               ForMember(m => m.Address, opt =>
               opt.MapFrom(m => m.Address));
            CreateMap<Customers, UserDto>().ReverseMap().
                ForMember(m => m.Address, opt =>
                opt.MapFrom(m => m.Address));

        }
    }
}