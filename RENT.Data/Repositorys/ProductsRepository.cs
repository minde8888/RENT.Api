using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RENT.Data.Context;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Data.Repositorys
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductsRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddProductsAsync(ProducRequesttDto product)
        {
            var cat = new Categories
            {
                CategoriesName = product.Category
            };
            _context.Categories.Add(cat);
            await _context.SaveChangesAsync();

            var products = new Products
            {
                ImageHeight = product.ImageHeight,
                ImageWidth = product.ImageWidth,
                ImageName = product.ImageName,
                Price = product.Price,
                Size = product.Size,
                Place = product.Place,
                Phone = product.Phone,
                Email = product.Email,
                CategoriesId = cat.CategoriesId,
                SellerId = product.SellerId
            };
            _context.Products.Add(products);
            await _context.SaveChangesAsync();

            var post = new Posts
            {
                ProductName = product.ProductName,
                Content = product.Content,
                ProductsId = products.ProductsId
            };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            var categories = new CategoriesProduct
            {
                ProductsId = products.ProductsId,
                CategoriesId = cat.CategoriesId
            };
            _context.CategoriesProduct.Add(categories);
            await _context.SaveChangesAsync();

            var contactForm = new ProductsContactForm
            {
                Name = product.Name,
                Email = product.Email,
                Phone = product.Phone
            };
            _context.ProductsContactForm.Add(contactForm);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetProductsAsync(string ImageSrc)
        {
            var products = await _context.Products.
                Include(c => c.Categories).
                Include(p => p.Posts).
                ToListAsync();

            var productsToReturn = _mapper.Map<List<ProductDto>>(products);

            foreach (var item in productsToReturn)
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

            return productsToReturn;
        }

        public async Task<List<Products>> GetProductIdAsync(Guid Id)
        {
            return await _context.Products.
                Include(c => c.Categories).
                Include(p => p.Posts).
                Include(f => f.ProductsContactForm).
                Where(x => x.ProductsId == Id).ToListAsync();
        }

        public async Task UpdateProductAsync(ProducRequesttDto productDto)
        {
            var products = await _context.Products.
                  Include(p => p.Posts).
                  Include(f => f.ProductsContactForm).
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
                products.DateUpdated = DateTime.UtcNow;

                products.Posts.Content = productDto.Content;
                products.Posts.ProductName = productDto.ProductName;
            }
            _context.Entry(products).State = EntityState.Modified;
            _context.Entry(products.Posts).State = EntityState.Modified;

            if (String.IsNullOrEmpty(productDto.Category))
            {
                var cats = await _context.CategoriesProduct.
                    Include(c => c.Categories).
                    Where(x => x.ProductsId == productDto.ProductsId).FirstOrDefaultAsync();

                cats.Categories.CategoriesName = productDto.Category;
                _context.Entry(cats).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        [HttpDelete("Delete/{id}")]
        public async Task RemoveProductsAsync([FromQuery]string id)
        {
            if (String.IsNullOrEmpty(id)) throw new Exception();

            Guid newId = new(id);

            var product = await _context.Products.
                Include(p => p.Posts).
                Include(f => f.ProductsContactForm).
                Where(x => x.ProductsId == newId).FirstOrDefaultAsync();

            product.IsDeleted = true;
            product.ProductsContactForm.IsDeleted = true;
            product.Posts.IsDeleted = true;

            await _context.SaveChangesAsync();
        }
    }
}