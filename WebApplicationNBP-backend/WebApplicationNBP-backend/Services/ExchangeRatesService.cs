using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplicationNBP_backend.Common;
using WebApplicationNBP_backend.Data;
using WebApplicationNBP_backend.Domain;
using WebApplicationNBP_backend.Domain.DTOs;
using WebApplicationNBP_backend.Interfaces;

namespace WebApplicationNBP_backend.Services
{
	public class ExchangeRatesService : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly IMemoryCacheService _memoryCacheService;

		public ExchangeRatesService(IServiceScopeFactory scopeFactory, IMemoryCacheService memoryCacheService)
		{
			_scopeFactory = scopeFactory;
			_memoryCacheService = memoryCacheService;

		}
		protected override async Task ExecuteAsync(CancellationToken cancelationToken)
		{
			await RunJob(cancelationToken);

			while (!cancelationToken.IsCancellationRequested)
			{
				var now = DateTime.Now;
				var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 12, 1, 0);

				if (now > nextRunTime)
				{
					nextRunTime = nextRunTime.AddDays(1);
				}

				var delay = nextRunTime - now;

				await Task.Delay(delay, cancelationToken);


				await RunJob(cancelationToken);
			}
		}

		private async Task RunJob(CancellationToken cancelationToken)
		{
			if (await CheckIfNewDataAbalible(cancelationToken))
			{
				await UpdateCurrencyTableInDataBase(cancelationToken);
				await SaveExchangeRatesToDataBase();
			}
		}

		private async Task<bool> CheckIfNewDataAbalible(CancellationToken cancelationToken)
		{
			using (var scope = _scopeFactory.CreateScope())
			{
				var nbpService = scope.ServiceProvider.GetRequiredService<INBPService>();
				var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

				var r = await nbpService.GetExchangeRatesFromTable("A");

				var nbpData = (await nbpService.GetExchangeRatesFromTable("A")).EffectiveDate;
				var dbDataItem = (await dbContext.ExchangeRatesTables.OrderByDescending(r => r.EffectiveDate).FirstOrDefaultAsync(cancelationToken));

				return dbDataItem is null || nbpData > dbDataItem.EffectiveDate;
			}
		}

		private async Task SaveExchangeRatesToDataBase()
		{
			var exchangeRatesTableDto = await _memoryCacheService.GetExchangeRatesTableFromCache();

			using (var scope = _scopeFactory.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

				var exchangeRatesTable = mapper.Map<ExchangeRatesTable>(exchangeRatesTableDto);

				dbContext.Add(exchangeRatesTable);
				dbContext.SaveChanges();
			}
		}

		private async Task UpdateCurrencyTableInDataBase(CancellationToken cancelationToken)
		{
			var currencyTableFromNBP = (await GetCurrenciesFromCache()).Select(r => new Currency { Name = r.Currency, Code = r.Code }).ToList();

			var currencyTableFromDataBase = await GetCurrenciesFromDataBase(cancelationToken);

			var missingCurrenciesInDataBase = currencyTableFromNBP.Except(currencyTableFromDataBase, new CurrencyComparer());

			if (missingCurrenciesInDataBase.Any())
			{
				using (var scope = _scopeFactory.CreateScope())
				{
					var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
					await dbContext.Currencies.AddRangeAsync(missingCurrenciesInDataBase);
					await dbContext.SaveChangesAsync(cancelationToken);
				}
			}
		}

		private async Task<List<Currency>> GetCurrenciesFromDataBase(CancellationToken cancelationToken)
		{
			using (var scope = _scopeFactory.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				return await context.Currencies.Select(r => new Currency { Name = r.Name, Code = r.Code }).ToListAsync(cancelationToken);
			}
		}

		private async Task<IEnumerable<RateDto>> GetCurrenciesFromCache()
		{
			return (await _memoryCacheService.AddExchangeRatesTableToCache()).Rates;
		}
	}
}
