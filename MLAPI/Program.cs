var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración para MercadoLibreApiClient
var mercadoLibreApiConfig = builder.Configuration.GetSection("MercadoLibreApi");
var baseAddress = mercadoLibreApiConfig["BaseUrl"];

if (string.IsNullOrWhiteSpace(baseAddress))
{
    throw new InvalidOperationException("MercadoLibre API BaseUrl is not configured in appsettings.json.");
}

builder.Services.AddHttpClient<MLAPI.Services.MercadoLibreApiClient>(client =>
{
    client.BaseAddress = new Uri(baseAddress);
});
// Fin de configuración para MercadoLibreApiClient

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
