using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebApplicationNBP_backend.Data;
using WebApplicationNBP_backend.Domain;
using WebApplicationNBP_backend.Domain.DTOs;
using WebApplicationNBP_backend.Interfaces;
using WebApplicationNBP_backend.Settings;

namespace WebApplicationNBP_backend.Services
{
	public class MemoryCacheService : IMemoryCacheService
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly HttpClientNBPSettings _httpClientSettings;
		private readonly ILogger<MemoryCacheService> _logger;
		public MemoryCacheService(IServiceProvider serviceProvider, IOptions<HttpClientNBPSettings> httpClientSettings, ILogger<MemoryCacheService> logger)
		{
			_serviceProvider = serviceProvider;
			_httpClientSettings = httpClientSettings.Value;
			_logger = logger;
		}

		public MemoryCache _cache { get; } = new MemoryCache(
			new MemoryCacheOptions
			{
				SizeLimit = 100
			});

		public async Task<ExchangeRatesTableDto> GetExchangeRatesTableFromCache()
		{
			int size = 1;

			_logger.LogInformation("Retrieving the exchange rate table from cache.");

			return await _cache.GetOrCreateAsync(
			   CacheKeys.ExchangeRatesTable, async opt =>
			   {
				   using (var scope = _serviceProvider.CreateScope())
				   {
					   var nbpService = scope.ServiceProvider.GetRequiredService<INBPService>();
					   ExchangeRatesTableDto exchangeRatesTable = null;
					   var rates = new List<RateDto>();

					   foreach (var item in _httpClientSettings.AvgExchangeRatesTables)
					   {
						   exchangeRatesTable = (await nbpService.GetExchangeRatesFromTable(item));
						   rates.AddRange(exchangeRatesTable.Rates);

					   }

					   opt.Size = size;

					   return new ExchangeRatesTableDto { EffectiveDate = exchangeRatesTable.EffectiveDate, Rates = rates };
				   }
			   });
		}

		public async Task<ExchangeRatesTableDto> AddExchangeRatesTableToCache()
		{
			int size = 1;

			var exchangeRates = await GetExchangeRatesTableFromCache();

			_cache.Set(CacheKeys.ExchangeRatesTable, exchangeRates, new MemoryCacheEntryOptions
			{
				Size = size
			});

			return exchangeRates;

		}

		public Dictionary<string, Currency> GetCurrencyFromCache()
		{
			int size = 1;

			return _cache.GetOrCreate(
			   CacheKeys.CurrencyTable, opt =>
			   {
				   using (var scope = _serviceProvider.CreateScope())
				   {
					   var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

					   opt.Size = size;

					   return context.Currencies.ToDictionary(r => r.Code);
				   }
			   });
		}
	}
}
