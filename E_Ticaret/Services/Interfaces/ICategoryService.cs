using Common.Model;

namespace E_Ticaret.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryModel>> GetNestedCategoriesAsync();
    }
}
