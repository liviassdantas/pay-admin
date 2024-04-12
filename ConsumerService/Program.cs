using ConsumerService.Service;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddHostedService<KafkaConsumerService>();

builder.Services.AddControllers();


var app = builder.Build();

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();

