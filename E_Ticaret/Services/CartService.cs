using Common.Model;
using E_Ticaret.Contexts;
using E_Ticaret.Domains.Entities;
using E_Ticaret.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Services
{
    public class CartService : ICartService
    {
        private readonly CoreContext _context;

        public CartService(CoreContext context)
        {
            _context = context;
        }

        public async Task<CartModel?> LoadCartFromDatabaseAsync(string userIdOrAnonId)
        {
            Cart? cartEntity;

            if (long.TryParse(userIdOrAnonId, out var userId))
            {
                cartEntity = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
            }
            else if (Guid.TryParse(userIdOrAnonId, out var anonId))
            {
                cartEntity = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.AnonymousId == anonId);
            }
            else
            {
                return new CartModel
                {
                    Id = 1,
                    CartItems = new List<CartItemModel>
                    {
                        new CartItemModel
                        {
                            Id = 1,
                            ProductId = 1,
                            Quantity = 2
                        },
                        new CartItemModel
                        {
                            Id = 2,
                            ProductId = 2,
                            Quantity = 1
                        },
                    }
                };
            }

            if (cartEntity == null) return null;

            return new CartModel
            {
                Id = cartEntity.Id,
                CartItems = cartEntity.CartItems.Select(item => new CartItemModel
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };
        }
    }
}
