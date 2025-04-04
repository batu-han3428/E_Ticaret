namespace E_Ticaret.Services.Interfaces
{
    public interface ITokenService
    {
        void GenerateTokens(HttpContext context);
    }
}
