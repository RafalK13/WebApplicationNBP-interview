using AutoMapper;
using WebApplicationNBP_backend.Domain;
using WebApplicationNBP_backend.Domain.DTOs;
using WebApplicationNBP_backend.Exceptions;
using WebApplicationNBP_backend.Interfaces;

namespace WebApplicationNBP_backend.Resolvers
{
	public class CurrencyResolver : IValueResolver<RateDto, Rate, int>
	{

		private readonly IMemoryCacheService _memoryCacheService;
		private readonly ILogger<CurrencyResolver> _logger;
		public CurrencyResolver(IMemoryCacheService memoryCacheService, ILogger<CurrencyResolver> logger)
		{
			_memoryCacheService = memoryCacheService;
			_logger = logger;
		}

		public int Resolve(RateDto source, Rate destination, int destMember, ResolutionContext context)
		{
			var currencies = _memoryCacheService.GetCurrencyFromCache();

			if (currencies.TryGetValue(source.Code, out var currency))
			{
				return currency.Id;
			}

			_logger.LogError($"Currency code {source.Code} not found in cache.");
			throw new CurrencyNotFoundException(source.Code);
		}
	}
}
