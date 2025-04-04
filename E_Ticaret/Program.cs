using E_Ticaret.Extension;
using Caching.Helpers;
using E_Ticaret.Middleware;
using E_Ticaret.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region MySql
builder.Services.MySqlSettings(builder.Configuration);
#endregion

#region Redis
builder.Services.AddRedisCache(builder.Configuration);
#endregion

#region Default Configuration Service
builder.Services.DefaultConfigurationService(builder.Configuration);
#endregion

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendOnly",
        builder => builder.WithOrigins("http://localhost:3500", "http://localhost:3502")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials());
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//builder.Services.AddDistributedMemoryCache();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

#region Middleware
app.UseCors("AllowFrontendOnly");

app.UseSession();

app.UseWhen(context => !context.Request.Path.StartsWithSegments("/api/security/csrf-token") &&
                            !context.Request.Path.StartsWithSegments("/cartHub") &&
                            !context.Request.Path.StartsWithSegments("/cartHub/negotiate"),
        appBuilder =>
        {
            appBuilder.UseMiddleware<SecurityMiddleware>();
            appBuilder.UseMiddleware<AnonymIdMiddleware>();
        });
#endregion

app.UseForwardedHeaders();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.UseStaticFiles();

app.MapControllers();

app.MapHub<CartHub>("/cartHub");

app.Run();
