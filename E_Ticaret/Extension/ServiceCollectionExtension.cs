using E_Ticaret.Contexts;
using E_Ticaret.Helpers;
using E_Ticaret.Helpers.Interfaces;
using E_Ticaret.Services;
using E_Ticaret.Services.Interfaces;

namespace E_Ticaret.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection MySqlSettings(this IServiceCollection services, IConfiguration Configuration)
        {
            var activeSql = Configuration["AppConfig:ActiveSql"];
            var conn = $"{activeSql}_DefaultConnection";

            services.ConfigureMysqlPooled<CoreContext>(Configuration, connectionString: conn, MigrationsAssembly: "E_Ticaret");

            return services;
        }

        public static IServiceCollection DefaultConfigurationService(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
