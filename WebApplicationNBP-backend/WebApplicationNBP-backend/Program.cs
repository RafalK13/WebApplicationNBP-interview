using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using WebApplicationNBP_backend.Data;
using WebApplicationNBP_backend.Interfaces;
using WebApplicationNBP_backend.Mapper;
using WebApplicationNBP_backend.Repositories;
using WebApplicationNBP_backend.Resolvers;
using WebApplicationNBP_backend.Services;
using WebApplicationNBP_backend.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.Configure<HttpClientNBPSettings>(builder.Configuration.GetSection(nameof(HttpClientNBPSettings)));

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin", builder =>
		builder.WithOrigins("http://localhost:3000")
			   .AllowAnyMethod()
			   .AllowAnyHeader());
});

var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient("HttpClientNBP", (serviceProvider, client) =>
{
	var httpClientSettings = serviceProvider.GetRequiredService<IOptions<HttpClientNBPSettings>>().Value;
	client.BaseAddress = new Uri(httpClientSettings.BaseAddress);
	client.DefaultRequestHeaders.Add("Accept", "application/xml");
});

builder.Services.AddScoped<INBPService, NBPService>();
builder.Services.AddScoped<IExchangeRatesRepository, ExchangeRatesRepository>();
builder.Services.AddScoped<CurrencyResolver>();
builder.Services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(Mapper));
builder.Services.AddHostedService<ExchangeRatesService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();
