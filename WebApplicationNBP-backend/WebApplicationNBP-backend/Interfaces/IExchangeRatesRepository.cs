using WebApplicationNBP_backend.Domain.DTOs;

namespace WebApplicationNBP_backend.Interfaces
{
	public interface IExchangeRatesRepository
	{
		Task<ExchangeRatesTableDto> GetAllExchangeRates();
	}
}
