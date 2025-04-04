using E_Ticaret.Helpers.Interfaces;

namespace E_Ticaret.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetUserId()
        {
            var context = _httpContextAccessor.HttpContext;
            var userId = context?.Items["UserId"]?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                userId = context?.Request.Cookies["anonId"];
            }

            return userId;
        }
    }
}
