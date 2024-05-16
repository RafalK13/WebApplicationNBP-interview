using WebApplicationNBP_backend.Domain.DTOs;
using WebApplicationNBP_backend.Interfaces;

namespace WebApplicationNBP_backend.Repositories
{
	public class ExchangeRatesRepository : IExchangeRatesRepository
	{
		private readonly IMemoryCacheService _memoryCacheService;

		public ExchangeRatesRepository(IMemoryCacheService memoryCacheService)
		{
			_memoryCacheService = memoryCacheService;
		}

		public async Task<ExchangeRatesTableDto> GetAllExchangeRates()
			=> await _memoryCacheService.GetExchangeRatesTableFromCache();
	}
}