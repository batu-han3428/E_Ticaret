using Caching.Services.Interfaces;
using Common.Model;
using E_Ticaret.Contexts;
using E_Ticaret.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Services
{
    public class ProductService: IProductService
    {
        private readonly CoreContext _context;
        private readonly IRedisCacheService _cacheService;

        public ProductService(CoreContext context, IRedisCacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Belirtilen kategoriye ait ürünleri getirir.
        /// Önce Redis'te kontrol eder, yoksa veritabanından çekip Redis'e kaydeder.
        /// </summary>
        public async Task<List<ProductModel>> GetProductsByCategoryAsync(int categoryId)
        {
            string cacheKey = $"CategoryProducts:{categoryId}";

            var cachedProducts = await _cacheService.GetAsync<List<ProductModel>>(cacheKey);
            if (cachedProducts != null)
                return cachedProducts;

            var products = await _context.ProductCategories
                .Where(pc => pc.CategoryId == categoryId)
                .Include(pc => pc.Product)
                .Select(pc => new ProductModel
                {
                    Id = pc.Product.Id,
                    Name = pc.Product.Name,
                    Price = pc.Product.Price,
                    StockQuantity = pc.Product.StockQuantity,
                    ImageUrl = pc.Product.ImageUrl,
                }).ToListAsync();

            await _cacheService.SetAsync(cacheKey, products);

            return products;
        }
    }
}
