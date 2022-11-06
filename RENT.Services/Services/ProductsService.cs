using AutoMapper;
using Org.BouncyCastle.Asn1.Ocsp;
using RENT.Data.Filter;
using RENT.Data.Interfaces.IRepositories;
using RENT.Data.Interfaces.IServices;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Dtos.ResponseDto;
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
        public async Task AddProductWithImage(ProductsRequestDto product)
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

        public async Task<ProductResponseDto> GetAllProductsAsync(PaginationFilter filter, string ImageSrc, string route)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            return await _productsRepository.GetProductsAsync(ImageSrc, validFilter, route);
        }
        public async Task<ProductsDto> GetProductById(Guid userId, string imageSrc)
        {
            var product = await _productsRepository.GetProductIdAsync(userId);
            if (product == null)
                 throw new Exception();
            return GetProductImage(product, imageSrc);
        }

        public async Task<ProductsDto> UpdateItemAsync(ProductsRequestDto product, string imageSrc)
        {
            if (product.Images != null)
            {
                string[] height = product.ImageHeight.Split(',');
                string[] width = product.ImageWidth.Split(',');
                string[] imageName = product.ImageSrc.Split(',');

                int count = 0;

                for (int i = 0; i < imageName.Length; i++)
                {
                    if (imageName[i] == "/")
                    {
                        imageName[i] = _imagesService.SaveImage(product.Images[count], height[count], width[count]).Replace(",", "");
                        count++;
                    }
                }
                product.ImageSrc = String.Join(',', imageName);
            }
            await _productsRepository.UpdateProductAsync(product);

            var response = await GetProductById(product.ProductsId, imageSrc);
            return response;
        }

        private ProductsDto GetProductImage(Products products, string imageSrc)
        {
            var productsToReturn = _mapper.Map<ProductsDto>(products);
            var newImages = new List<string>();        
            string[] ImageName = productsToReturn.ImageName.Split(',');
                foreach (var img in ImageName)
                {
                    if (!String.IsNullOrEmpty(img))
                    {
                        newImages.Add(String.Format("{0}/Images/{1}", imageSrc, img));
                        productsToReturn.ImageSrc = newImages;
                    }
                }            
            return productsToReturn;
        }
    }
}
