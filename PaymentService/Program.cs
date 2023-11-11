using Microsoft.OpenApi.Models;
using pay_admin.Interfaces;
using pay_admin.Model;
using pay_admin.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<PaymentsDatabaseSettings>(
    builder.Configuration.GetSection("PaymentsDatabase"));

builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "A Payment Administrator Service.",
        Description = "The shoppers and the managers can cancel and create payments transactions."
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.MapControllers();

app.Run();

