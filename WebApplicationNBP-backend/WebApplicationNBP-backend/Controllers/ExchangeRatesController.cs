using Microsoft.AspNetCore.Mvc;
using WebApplicationNBP_backend.Interfaces;

namespace WebApplicationNBP_backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ExchangeRatesController : Controller
	{
		private readonly IExchangeRatesRepository _currencyRepository;

		public ExchangeRatesController(IExchangeRatesRepository currencyRepository)
		{
			_currencyRepository = currencyRepository;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllCurrencies()
		{
			var exchangeRates = await _currencyRepository.GetAllExchangeRates();

			return Ok(exchangeRates);
		}
	}
}
