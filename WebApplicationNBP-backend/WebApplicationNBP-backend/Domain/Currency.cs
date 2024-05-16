namespace WebApplicationNBP_backend.Domain
{
	public class Currency
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public ICollection<Rate> Rates { get; set; } = new List<Rate>();
	}
}
