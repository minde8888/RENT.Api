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

        public List<ProductDto> GetProductImage(List<Products> products, string imageSrc)
        {
            var productsToReturn = _mapper.Map<List<ProductDto>>(products);

            var newImages = new List<string>();

            foreach (var item in productsToReturn)
            {
                string[] ImageName = item.ImageName.Split(',');
          
                foreach (var img in ImageName)
                {
                    if (!String.IsNullOrEmpty(img))
                    {
                        newImages.Add(String.Format("{0}/Images/{1}", imageSrc, img));
                        item.ImageSrc = newImages;
                    }
                }
            }
            return productsToReturn;
        }

    }
}
