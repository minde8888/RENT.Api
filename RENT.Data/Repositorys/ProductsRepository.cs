using AutoMapper;
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
        public async Task AddProductsAsync(ProductDto product)
        {
            var productsToSave = _mapper.Map<Products>(product);
            _context.Products.Add(productsToSave);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetProductsAsync(string ImageSrc)
        {
            var products = await _context.Products.
                Include(c => c.Categories).
                Include(p => p.Posts).
                Include(ps => ps.Specifications).
                ToListAsync();

            var productsToReturn = _mapper.Map<List<ProductDto>>(products);

            foreach (var item in productsToReturn)
            {
                foreach (var product in item.ImageName)
                {
                    item.ImageSrc.Add(String.Format("{0}/Images/{1}", ImageSrc, product));
                }

            }
            return productsToReturn;
        }

        public async Task<List<Products>> GetProductIdAsync(Guid Id)
        {
            return await _context.Products.
                Include(c => c.Categories).
                Include(p => p.Posts).
                Include(ps => ps.Specifications).
                Where(x => x.ProductsId == Id).ToListAsync();
        }

        public async Task RemoveProductsAsync(Guid userId)
        {
            var product = _context.Products
                .Where(x => x.ProductsId == userId).FirstOrDefault();
            product.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}