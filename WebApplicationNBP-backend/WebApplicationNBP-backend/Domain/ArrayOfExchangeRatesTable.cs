namespace WebApplicationNBP_backend.Domain
{
	public class ArrayOfExchangeRatesTable
	{
		public int Id { get; set; }
		public ICollection<ExchangeRatesTable> ExchangeRatesTables { get; set; } = new List<ExchangeRatesTable>();
	}
}
