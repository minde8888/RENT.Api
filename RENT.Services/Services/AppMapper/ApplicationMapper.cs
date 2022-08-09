using AutoMapper;
using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Services.Services.AppMapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Seller, UserRegistrationDto>().ReverseMap();

            CreateMap<Seller, UserInformationDto>().ReverseMap().
                ForMember(m => m.Address, opt =>
               opt.MapFrom(m => m.AddressDto));

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<Seller, UserRegistrationDto>().ReverseMap();

            CreateMap<Seller, UserInformationDto>().ReverseMap().
                ForMember(m => m.Address, opt =>
               opt.MapFrom(m => m.AddressDto));

            CreateMap<Seller, UserDto>().ReverseMap().
               ForMember(m => m.Address, opt =>
               opt.MapFrom(m => m.AddressDto));

            CreateMap<Customers, UserRegistrationDto>().ReverseMap();
            CreateMap<Customers, UserInformationDto>().ReverseMap();

            CreateMap<Customers, UserDto>().ReverseMap().
               ForMember(m => m.Address, opt =>
               opt.MapFrom(m => m.AddressDto));

            CreateMap<Customers, UserDto>().ReverseMap().
                ForMember(m => m.Address, opt =>
                opt.MapFrom(m => m.AddressDto));

            CreateMap<Products, ProductDto>().ReverseMap().
                ForMember(c => c.Posts, opt => opt.MapFrom(d => d.PostsDto)).
                ForMember(c => c.Categories, opt => opt.MapFrom(d => d.CategoriesDto));
        }
    }
}