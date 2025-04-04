using Common.Model;

namespace E_Ticaret.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartModel?> LoadCartFromDatabaseAsync(string userIdOrAnonId);
    }
}
