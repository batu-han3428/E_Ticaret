using Caching.Services.Interfaces;
using Common.Model;
using E_Ticaret.Contexts;
using E_Ticaret.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CoreContext _context;
        private readonly IRedisCacheService _cacheService;

        public CategoryService(CoreContext context, IRedisCacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Kategorileri iç içe olacak şekilde getirir.
        /// Önce Redis'te kontrol eder, yoksa veritabanından çekip Redis'e kaydeder.
        /// </summary>
        public async Task<List<CategoryModel>> GetNestedCategoriesAsync()
        {
            string cacheKey = "Categories";

            var cachedCategories = await _cacheService.GetAsync<List<CategoryModel>>(cacheKey);
            if (cachedCategories != null)
                return cachedCategories;

            var categories = await _context.Categories.Where(x=> x.IsActive && !x.IsDeleted).AsNoTracking().ToListAsync();
            var categoryRelations = await _context.CategoryRelations.AsNoTracking().ToListAsync();

            var categoryDict = categories.ToDictionary(cat => cat.Id, cat => new CategoryModel
            {
                Id = cat.Id,
                Name = cat.Name,
                ChildCategories = new List<CategoryModel>()
            });

            foreach (var relation in categoryRelations)
            {
                if (categoryDict.ContainsKey(relation.ParentCategoryId) && categoryDict.ContainsKey(relation.ChildCategoryId))
                {
                    categoryDict[relation.ParentCategoryId].ChildCategories.Add(categoryDict[relation.ChildCategoryId]);
                }
            }

            var nestedCategories = categoryDict.Values.Where(cat =>
                !categoryRelations.Any(rel => rel.ChildCategoryId == cat.Id)).ToList();

            await _cacheService.SetAsync(cacheKey, nestedCategories);

            return nestedCategories;
        }
    }
}
