using AuthService.Interfaces;
using AuthService.Model;
using AuthService.Service;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AuthDatabaseSettings>(
    builder.Configuration.GetSection("AuthDatabase"));

builder.Services.AddScoped<IAuthService, AuthServices>();

builder.Services.AddControllers();

builder.Services.AddAuthentication().AddJwtBearer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "An Authentication Service",
        Description = "Made a valid login in an API."
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
