namespace E_Ticaret.Middleware
{
    public class AnonymIdMiddleware
    {
        private readonly RequestDelegate _next;

        public AnonymIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                var userId = "user_id_from_token";  // JWT doğrulama burada yapılacak

                context.Items["UserId"] = userId;
            }
            else
            {
                var anonId = context.Request.Cookies["anonId"];

                if (string.IsNullOrEmpty(anonId))
                {
                    anonId = Guid.NewGuid().ToString();
                    context.Response.Cookies.Append("anonId", anonId, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        HttpOnly = true
                    });
                }

                context.Items["UserId"] = anonId;
            }

            await _next(context);
        }
    }
}
