using Caching.Services.Interfaces;
using Common.Model;
using E_Ticaret.Helpers.Interfaces;
using E_Ticaret.Hubs;
using E_Ticaret.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private IRedisCacheService _cacheService;
        private readonly IUserHelper _userHelper;
        private readonly ICartService _cartService;
        private readonly IHubContext<CartHub> _hubContext;

        public CartController(IRedisCacheService cacheService, IUserHelper userHelper, ICartService cartService, IHubContext<CartHub> hubContext)
        {
            _cacheService = cacheService;
            _userHelper = userHelper;
            _cartService = cartService;
            _hubContext = hubContext;
        }

        [HttpGet("GetCart")]
        public async Task<IActionResult> GetCart()
        {
            var userIdOrAnonId = _userHelper.GetUserId();

            if (string.IsNullOrEmpty(userIdOrAnonId))
            {
                return BadRequest("User ID or Anonymous ID is missing.");
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

            return Ok(cart);
        }

        [HttpPost("UpdateCart")]
        public async Task<IActionResult> UpdateCart([FromBody] CartModel cart)
        {
            var userIdOrAnonId = _userHelper.GetUserId();
            if (string.IsNullOrEmpty(userIdOrAnonId))
                return BadRequest("User ID or Anonymous ID is missing.");

            await _cacheService.SetAsync($"Cart:{userIdOrAnonId}", cart);

            await _hubContext.Clients.User(userIdOrAnonId).SendAsync("ReceiveCartUpdate");

            return Ok();
        }
    }
}
