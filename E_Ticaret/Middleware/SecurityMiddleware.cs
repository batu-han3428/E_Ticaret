namespace E_Ticaret.Middleware
{
    public class SecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _allowedFrontendUrl;
        private readonly string _apiKey;

        public SecurityMiddleware(RequestDelegate next)
        {
            _next = next;
            _allowedFrontendUrl = Environment.GetEnvironmentVariable("ALLOWED_FRONTEND_URL") ?? "http://localhost:3502";
            _apiKey = Environment.GetEnvironmentVariable("API_KEY") ?? "default-api-key";
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var referer = context.Request.Headers["Referer"].ToString();
            var origin = context.Request.Headers["Origin"].ToString();
            var apiKey = context.Request.Cookies["API-KEY"];
            var csrfToken = context.Request.Cookies["CSRF-TOKEN"];
            var storedCsrfToken = context.Session.GetString("CSRF-SESSION");

            if (!referer.StartsWith(_allowedFrontendUrl) && !origin.StartsWith(_allowedFrontendUrl))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Yetkisiz domain!");
                return;
            }

            if (apiKey == null || apiKey != _apiKey)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Geçersiz API anahtarı!");
                return;
            }

            if (csrfToken == null || csrfToken != storedCsrfToken)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Geçersiz CSRF token!");
                return;
            }

            await _next(context);
        }
    }
}
