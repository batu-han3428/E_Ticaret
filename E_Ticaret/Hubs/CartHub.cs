using Caching.Services.Interfaces;
using Common.Model;
using E_Ticaret.Helpers.Interfaces;
using E_Ticaret.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace E_Ticaret.Hubs
{
    public class CartHub : Hub
    {
        private readonly ICartService _cartService;
        private readonly IRedisCacheService _cacheService;
        private readonly IUserHelper _userHelper;

        public CartHub(ICartService cartService, IRedisCacheService cacheService, IUserHelper userHelper)
        {
            _cartService = cartService;
            _cacheService = cacheService;
            _userHelper = userHelper;
        }

        public async Task NotifyCartUpdated()
        {
            var userIdOrAnonId = _userHelper.GetUserId();

            if (string.IsNullOrEmpty(userIdOrAnonId))
            {
                await Clients.Caller.SendAsync("ReceiveCartUpdateError", "User ID or Anonymous ID is missing.");
                return;
            }

            var cart = await _cacheService.GetAsync<CartModel>($"Cart:{userIdOrAnonId}");

            if (cart == null)
            {
                cart = await _cartService.LoadCartFromDatabaseAsync(userIdOrAnonId);

                if (cart == null)
                {
                    cart = new CartModel
                    {
                        Id = 0,
                        CartItems = new List<CartItemModel>()
                    };
                }

                await _cacheService.SetAsync($"Cart:{userIdOrAnonId}", cart);
            }

            await Clients.Caller.SendAsync("ReceiveCartUpdate", cart);
        }
    }

}
