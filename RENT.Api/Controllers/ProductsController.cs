﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Dtos.ResponseDto;
using RENT.Services.Services;

namespace RENT.Api.Controllers
{
    public class ProductsController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImagesService _imagesService;
        private readonly IMapper _mapper;
        private readonly IProductsRepository _productsRepository;

        public ProductsController(IMapper mapper, ImagesService imagesService, IWebHostEnvironment hostEnvironment)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
            _imagesService = imagesService ?? throw new ArgumentNullException(nameof(_imagesService));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(_hostEnvironment));
        }

        [HttpPost]
        [Authorize(Roles = "Seller, Admin")]
        public async Task<ActionResult<ResponseProductsDto>> AddNewproduct([FromForm] RequestProductsDto product)
        {
            try
            {
                if (!String.IsNullOrEmpty(product.ImageName))
                {
                    string path = _hostEnvironment.ContentRootPath;
                    var imageName = _imagesService.SaveImage(product.ImageFile, product.Height, product.Width);
                }
                await _productsRepository.AddProductsAsync(product);

                return CreatedAtAction("Get", new { product.ProductsId }, product);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error Get data from the database -> AddNewEmployee");
            }
        }

    }
}
