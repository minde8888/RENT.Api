using AutoMapper;
using RENT.Data.Context;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Data.Repositorys
{
    public class ProductsRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductsRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddProductsAsync(RequestProductsDto product)
        {
            var productsToSave = _mapper.Map<Products>(product);
            _context.Products.Add(productsToSave);
            await _context.SaveChangesAsync();
        }
    }
}