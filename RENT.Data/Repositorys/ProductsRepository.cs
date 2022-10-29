using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RENT.Data.Context;
using RENT.Data.Filter;
using RENT.Data.Helpers;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Dtos.ResponseDto;
using RENT.Domain.Entities;

namespace RENT.Data.Repositorys
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public ProductsRepository(AppDbContext context, IMapper mapper, IUriService uriService)
        {
            _context = context;
            _mapper = mapper;
            _uriService = uriService;
        }

        public async Task AddProductsAsync(ProducRequestDto product)
        {
            Products products = new()
            {
                ImageHeight = product.ImageHeight,
                ImageWidth = product.ImageWidth,
                ImageName = product.ImageName,
                Price = product.Price,
                Size = product.Size,
                Place = product.Place,
                Phone = product.Phone,
                Email = product.Email,
                SellerId = product.SellerId
            };
            _context.Products.Add(products);
            await _context.SaveChangesAsync();

            Posts post = new()
            {
                ProductName = product.ProductName,
                Content = product.ProductDescription,
                ProductsId = products.ProductsId
            };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            string[] categories = product.CategoriesName.Split(',');

            foreach (var item in categories)
            {
                Categories cat = new();
                cat.CategoriesName = item;
                _context.Categories.Add(cat);
                await _context.SaveChangesAsync();

                CategoriesProduct category = new()
                {
                    ProductsId = products.ProductsId,
                    CategoriesId = cat.CategoriesId
                };
                _context.CategoriesProduct.Add(category);
                await _context.SaveChangesAsync();

            }
        }

        public async Task<ProductResponseDto> GetProductsAsync(string ImageSrc, PaginationFilter validFilter, string route)
        {
            var pagedData = await _context.Products
                 .Include(c => c.Categories)
                 .Include(p => p.Posts)
                 .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                 .Take(validFilter.PageSize)
                 .ToListAsync();

            var totalRecords = await _context.Products
                 .Include(c => c.Categories)
                 .Include(p => p.Posts)
                 .CountAsync();

            var products = PaginationHelper.CreatePagedReponse<Products>(pagedData, validFilter, totalRecords, _uriService, route);

            ProductResponseDto response = new();
            response.PageNumber = products.PageNumber;
            response.PageSize = products.PageSize;
            response.TotalPages = products.TotalPages;
            response.TotalRecords = products.TotalRecords;
            response.NextPage = products.NexPage;
            response.PreviousPage = products.PrevPage;
            response.ProductDto = _mapper.Map<List<ProductDto>>(products.Data);

            foreach (var item in response.ProductDto)
            {
                string[] name = item.ImageName.Split(','); ;
                var newImages = new List<string>();

                foreach (var img in name)
                {
                    if (!String.IsNullOrEmpty(img))
                    {
                        newImages.Add(string.Format("{0}/Images/{1}", ImageSrc, img));
                        item.ImageSrc = newImages;
                    }
                }
            }
            return response;
        }

        public async Task<Products> GetProductIdAsync(Guid Id)
        {
            return await _context.Products.
                Include(c => c.Categories).
                Include(p => p.Posts).
                Where(x => x.ProductsId == Id).FirstOrDefaultAsync();
        }

        public async Task UpdateProductAsync(ProducRequestDto productDto)
        {
            var products = await _context.Products.
                  Include(p => p.Posts).
                  Where(x => x.ProductsId == productDto.ProductsId).FirstOrDefaultAsync();

            if (products != null)
            {
                products.ImageName = productDto.ImageSrc;
                products.Email = productDto.Email;
                products.Price = productDto.Price;
                products.Phone = productDto.Phone;
                products.ImageWidth = productDto.ImageWidth;
                products.ImageHeight = productDto.ImageHeight;
                products.Place = productDto.Place;
                products.Size = productDto.Size;
                products.DateUpdated = DateTime.UtcNow;

                products.Posts.Content = productDto.ProductDescription;
                products.Posts.ProductName = productDto.ProductName;
            }
            _context.Entry(products).State = EntityState.Modified;
            _context.Entry(products.Posts).State = EntityState.Modified;

            if (productDto.CategoriesName != null)
            {
                var cats = _context.CategoriesProduct.
                  Include(c => c.Categories).
                  Where(x => x.ProductsId == productDto.ProductsId);
                _context.CategoriesProduct.RemoveRange(cats);
                _context.SaveChanges();

                string[] category = productDto.CategoriesName.Split(',');

                foreach (var item in category)
                {
                    Categories categories = new()
                    {
                        CategoriesName = item,
                    };
                    _context.Categories.Add(categories);
                    await _context.SaveChangesAsync();

                    CategoriesProduct categoriesProduct = new()
                    {
                        CategoriesId = categories.CategoriesId,
                        ProductsId = productDto.ProductsId,
                    };
                    _context.CategoriesProduct.Add(categoriesProduct);
                    await _context.SaveChangesAsync();
                }
            }
            await _context.SaveChangesAsync();
        }

        [HttpDelete("Delete/{id}")]
        public async Task RemoveProductsAsync(string id)
        {
            if (String.IsNullOrEmpty(id)) throw new Exception();

            Guid newId = new(id);

            var product = await _context.Products.
                Include(p => p.Posts).
                Where(x => x.ProductsId == newId).FirstOrDefaultAsync();

            product.IsDeleted = true;
            product.Posts.IsDeleted = true;

            await _context.SaveChangesAsync();
        }
    }
}