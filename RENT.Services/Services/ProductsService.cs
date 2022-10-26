using AutoMapper;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Services.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _productsRepository;
        private readonly IImagesService _imagesService;

        public ProductsService(IProductsRepository productsRepository, IImagesService imagesService, IMapper mapper)
        {
            _mapper = mapper;
            _productsRepository = productsRepository;
            _imagesService = imagesService;
        }

        public async Task AddProductWithImage(ProducRequestDto product)
        {
            var imageName = "";
            if (product.Images != null)
            {
                string[] height = product.ImageHeight.Split(',');
                string[] width = product.ImageWidth.Split(',');

                for (int i = 0; i < height.Length; i++)
                {
                    imageName += _imagesService.SaveImage(product.Images[i], height[i], width[i]);
                }
            }
            product.ImageName = imageName;
            await _productsRepository.AddProductsAsync(product);
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
