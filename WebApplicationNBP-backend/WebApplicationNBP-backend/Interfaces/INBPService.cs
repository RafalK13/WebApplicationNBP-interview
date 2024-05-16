using WebApplicationNBP_backend.Domain.DTOs;

namespace WebApplicationNBP_backend.Interfaces
{
	public interface INBPService
	{
		Task<ExchangeRatesTableDto> GetExchangeRatesFromTable(string table);
	}
}
