using AutoMapper;
using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Dtos.ResponseDto;
using RENT.Domain.Entities;
using RENT.Domain.Entities.Wrappers;

namespace RENT.Services.Services.AppMapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Seller, UserRegistrationDto>().ReverseMap();

            CreateMap<Seller, UserInformationDto>().ReverseMap();

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<Seller, UserRegistrationDto>().ReverseMap();

            CreateMap<Seller, UserInformationDto>().ReverseMap();

            CreateMap<Seller, UserDto>().ReverseMap();

            CreateMap<Customers, UserRegistrationDto>().ReverseMap();

            CreateMap<Customers, UserInformationDto>().ReverseMap();

            CreateMap<Customers, UserDto>().ReverseMap();

            CreateMap<Customers, UserDto>().ReverseMap();

            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<Categories, CategoriesDto>().ReverseMap();
            CreateMap<Products, ProductsDto>().
                ForMember(dest => dest.PostDto, act => act.MapFrom(src => src.Post)).
                ForMember(dest => dest.CategoriesDto, act => act.MapFrom(src => src.Categories)).ReverseMap();

            CreateMap<PagedResponse<Products>, ProductResponseDto>().ReverseMap().
               ForMember(m => m.Data, opt =>
               opt.MapFrom(m => m.ProductDto));
        }
    }
}