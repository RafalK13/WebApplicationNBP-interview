using WebApplicationNBP_backend.Domain;
using WebApplicationNBP_backend.Domain.DTOs;

namespace WebApplicationNBP_backend.Interfaces
{
	public interface IMemoryCacheService
	{
		Task<ExchangeRatesTableDto> GetExchangeRatesTableFromCache();

		Task<ExchangeRatesTableDto> AddExchangeRatesTableToCache();

		Dictionary<string, Currency> GetCurrencyFromCache();
	}
}
