using Common.Model;

namespace E_Ticaret.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductModel>> GetProductsByCategoryAsync(int categoryId);
    }
}
