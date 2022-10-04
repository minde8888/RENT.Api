using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RENT.Data.Context;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Data.Repositorys
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CategoriesDto> AddCategotyAsync(CategoriesDto category)
        {
            Categories cat = new ()
            {
                CategoriesName = category.CategoriesName,
            };
            _context.Categories.Add(cat);
            await _context.SaveChangesAsync();

            CategoriesProduct categories = new()
            {
                ProductsId = category.ProductsId,
                CategoriesId = cat.CategoriesId
            };
            _context.CategoriesProduct.Add(categories);
            await _context.SaveChangesAsync();
           return _mapper.Map<CategoriesDto>(cat);

        }
        public void RemoveCategoryAsync(string id)
        {
            var guid = new Guid(id);
            var cat =_context.Categories.Where(x => x.CategoriesId == guid).FirstOrDefault();
            _context.Categories.Remove(cat);
            _context.SaveChangesAsync();
        }
    }
}
