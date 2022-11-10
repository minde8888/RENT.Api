using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RENT.Data.Context;
using RENT.Data.Interfaces.IRepositories;
using RENT.Domain.Dtos;
using RENT.Domain.Entities;

namespace RENT.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(AppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CategoriesDto> AddCategoryAsync(CategoriesDto category)
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
            if (prodCat?.Categories == null) throw new ArgumentNullException("Could not get Categories from DB", nameof(prodCat));

            return _mapper.Map<CategoriesDto>(prodCat.Categories);
        }

        public async Task UpdateCategory(CategoriesDto category)
        {
            var categories = category.CategoriesName.Split(',');
            var categoriesId = category.CategoriesUpdateId.ToString().Split(',');

            for (var i = 0; i < categories.Length; i++)
            {
                var guid = new Guid(categoriesId[i]);
                var categorySave = _context.Categories
                    .Single(x => x.CategoriesId == guid);
                if (categorySave?.CategoriesName == null) throw new ArgumentNullException("Could not get Categories from DB", nameof(categorySave));

                categorySave.CategoriesName = categories[i];
                _context.Entry(categorySave).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public void RemoveCategoryAsync(string id)
        {
            var guid = new Guid(id);
            var cat = _context.Categories
                .Single(x => x.CategoriesId == guid);
            if (cat?.CategoriesName == null) throw new ArgumentNullException("Could not get Categories from DB", nameof(cat));

            _context.Categories.Remove(cat);
            _context.SaveChangesAsync();
        }
    }
}