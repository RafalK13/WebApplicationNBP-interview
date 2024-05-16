namespace WebApplicationNBP_backend.Domain
{
	public class ExchangeRatesTable
	{
		public int Id { get; set; }
		public DateTime EffectiveDate { get; set; }
		public ICollection<Rate> Rates { get; set; } = new List<Rate>();
	}
}
