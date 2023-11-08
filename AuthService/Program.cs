var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication().AddJwtBearer();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.Run();
