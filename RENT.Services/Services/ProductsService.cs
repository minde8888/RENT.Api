using AutoMapper;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Services.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IMapper _mapper;

        public ProductsService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<ProductDto> GetProductImageAsync(List<Products> products, string imageSrc)
        {
            var productsToReturn = _mapper.Map<List<ProductDto>>(products);

            foreach (var item in productsToReturn)
            {
                foreach (var product in item.ImageName)
                {
                    item.ImageSrc.Add(String.Format("{0}/Images/{1}", imageSrc, product));
                }

            }
            return productsToReturn;
        }

    }
}
