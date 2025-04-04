using E_Ticaret.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticaret.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public SecurityController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("csrf-token")]
        public IActionResult GetCsrfToken()
        {
            _tokenService.GenerateTokens(HttpContext);

            return Ok(new { message = "Yeni CSRF Token ve API Key gönderildi." });
        }
    }
}
