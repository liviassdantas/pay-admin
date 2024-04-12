using ConsumerService.Service;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var loggerFactory = new LoggerFactory();
builder.Services.AddHostedService(sp => new CreateABillingConsumerService(config, loggerFactory, "CreateABillingGroup", "CreateABilling"));

builder.Services.AddControllers();


var app = builder.Build();

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();

