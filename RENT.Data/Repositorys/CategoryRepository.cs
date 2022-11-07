using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RENT.Data.Context;
using RENT.Data.Interfaces.IRepositories;
using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;
using System;

namespace RENT.Data.Repository
{
    public class CategoryRepository : ICategoryRepository
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
            Categories cat = new()
            {
                CategoriesName = category.CategoriesName,
            };
            _context.Categories.Add(cat);
            await _context.SaveChangesAsync();

            CategoriesProducts categories = new()
            {
                ProductsId = category.ProductsId,
                CategoriesId = cat.CategoriesId
            };
            _context.CategoriesProduct.Add(categories);
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoriesDto>(cat);
        }

        public async Task<List<CategoriesDto>> GetAllCategoriesAsync()
        {
            var catList = await _context.CategoriesProduct.Select(_ => _mapper.Map<CategoriesDto>(_.Categories)).ToListAsync();
            return catList; 
        }
        public async Task<CategoriesDto> GetCategoriesIdAsync(Guid guidId)
        {
            var prodCat = await _context.CategoriesProduct
                .Where(x => x.CategoriesId == guidId).FirstOrDefaultAsync();
            return _mapper.Map<CategoriesDto>(prodCat.Categories);
        }
        public async Task UpdateCategory(CategoriesDto category)
        {
            string[] categories = category.CategoriesName.Split(',');
            string[] categoriesId = category.CategoriesUpdateId.ToString().Split(',');

            for (int i = 0; i < categories.Length; i++)
            {
                var guid = new Guid(categoriesId[i]);
                var categorySave = _context.Categories
                    .Where(x => x.CategoriesId == guid).FirstOrDefault();
                categorySave.CategoriesName = categories[i];
                _context.Entry(categorySave).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public void RemoveCategoryAsync(string id)
        {
            var guid = new Guid(id);
            var cat = _context.Categories
                .Where(x => x.CategoriesId == guid).FirstOrDefault();
            _context.Categories.Remove(cat);
            _context.SaveChangesAsync();
        }
    }
}
