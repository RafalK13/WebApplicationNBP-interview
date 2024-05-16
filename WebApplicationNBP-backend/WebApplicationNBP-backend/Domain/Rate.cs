namespace WebApplicationNBP_backend.Domain
{
	public class Rate
	{
		public int Id { get; set; }
		public int ExchangeRatesTableId { get; set; }
		public ExchangeRatesTable ExchangeRatesTable { get; set; }
		public int CurrencyId { get; set; }
		public Currency Currency { get; set; }
		public decimal MidRate { get; set; }
	}
}
