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
                ProductCode = product.ProductCode,
                CategoriesId = cat.CategoriesId,
                SellerId = product.SellerId
            };
            _context.Products.Add(products);
            await _context.SaveChangesAsync();

            var post = new Posts
            {
                ProductName = product.ProductName,
                Content = product.ProductDescription,
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

            //var productReturn = _mapper.Map<List<ProductDto>>(products);
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
                if (!item.ImageName.Any())
                {
                    foreach (var product in item.ImageName)
                    {
                        item.ImageSrc.Add(String.Format("{0}/Images/{1}", ImageSrc, product));
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
                Where(x => x.ProductsId == Id).ToListAsync();
        }

        public async Task<IList<ProductDto>> UpdateProductAsync(ProductDto productDto)
        {
            var products = await _context.Products.
                 Include(c => c.Categories).
                 Include(p => p.Posts).
                 Where(x => x.ProductsId == productDto.ProductsId).FirstOrDefaultAsync();

            if (products != null)
            {
                //products.ProductName = productDto.ProductName ?? products.ProductName;
                //products.QuantityPerUnit = productDto.QuantityPerUnit ?? products.QuantityPerUnit;
                //products.UnitPrice = productDto.UnitPrice ?? products.UnitPrice;
                //products.UnitsInStock = productDto.UnitsInStock ?? products.UnitsInStock;
                //products.WarehousePlace = productDto.WarehousePlace ?? products.WarehousePlace;
                products.DateUpdated = DateTime.UtcNow;
                products.Posts.Content = productDto.Posts.Content ?? products.Posts.Content;
                //products.Specifications.MaxLoad = productDto.Specifications.MaxLoad ?? products.Specifications.MaxLoad;
                //products.Specifications.Weight = productDto.Specifications.Weight ?? products.Specifications.Weight;
                //products.Specifications.LiftingHeight = productDto.Specifications.LiftingHeight ?? products.Specifications.LiftingHeight;
                //products.Specifications.Capacity = productDto.Specifications.Capacity ?? products.Specifications.Capacity;
                //products.Specifications.EnergySource = productDto.Specifications.EnergySource ?? products.Specifications.EnergySource;
                //products.Specifications.Speed = productDto.Specifications.Speed ?? products.Specifications.Speed;
                //products.Specifications.Length = productDto.Specifications.Length ?? products.Specifications.Length;
                //products.Specifications.Width = productDto.Specifications.Width ?? products.Specifications.Width;
                //products.Specifications.Height = productDto.Specifications.Height ?? products.Specifications.Height;
            }

            //if (String.IsNullOrEmpty(productDto.Categories))
            //{
            //    var toRemove = _context.ProductsCategories
            //        .Where(x => x.ProductsId == productDto.ProductsId);
            //    _context.ProductsCategories.RemoveRange(toRemove);
            //    _context.SaveChanges();

            //    string[] cat = productDto.Categories.Split(',');

            //    foreach (var id in cat)
            //    {
            //        var prodCat = new ProductsCategories
            //        {
            //            CategoriesId = new Guid(id.ToString()),
            //            ProductsId = productDto.ProductsId
            //        };
            //        _context.ProductsCategories.Add(prodCat);
            //        await _context.SaveChangesAsync();
            //    }
            //}

            _context.Entry(products).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var productsToReturn = _context.Products.
            Include(c => c.Categories).
            Include(p => p.Posts).
            Single(x => x.ProductsId == productDto.ProductsId);

            var productReturn = _mapper.Map<IList<ProductDto>>(productsToReturn);

            return productReturn;
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