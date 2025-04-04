using E_Ticaret.Services.Interfaces;

namespace E_Ticaret.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _apiKey;

        public TokenService(IConfiguration configuration)
        {
            _apiKey = configuration["SecuritySettings:ApiKey"] ?? "default-api-key";
        }

        public void GenerateTokens(HttpContext context)
        {
            var csrfToken = Guid.NewGuid().ToString();

            context.Session.SetString("CSRF-SESSION", csrfToken);

            context.Response.Cookies.Append("CSRF-TOKEN", csrfToken, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                //Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30)
                //SameSite = SameSiteMode.None,
                //Path = "/",
            });

            context.Response.Cookies.Append("API-KEY", _apiKey, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                //Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30)
                //SameSite = SameSiteMode.None
            });
        }
    }

}
