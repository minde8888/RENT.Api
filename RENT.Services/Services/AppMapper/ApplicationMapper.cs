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

            CreateMap<Products, ProductDto>()
                  .ForMember(dest => dest.CategoriesDto, opt => opt.MapFrom(src => src.ProductCode)).ReverseMap();
            //.ForMember(dest => dest.CategoriesDto, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Categories));
            //.ForMember(x => x.CategoriesDto, a => a.MapFrom(y => y.Categories.FirstOrDefault(b =>b.CategoriesId )).ReverseMap();
        }
    }
}