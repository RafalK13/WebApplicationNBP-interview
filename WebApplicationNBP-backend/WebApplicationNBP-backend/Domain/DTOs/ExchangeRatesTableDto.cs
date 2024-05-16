using System.Xml.Serialization;

namespace WebApplicationNBP_backend.Domain.DTOs
{
	[XmlRoot("ExchangeRatesTable")]
	public class ExchangeRatesTableDto
	{
		public string Table { get; set; }

		public string No { get; set; }

		public DateTime EffectiveDate { get; set; }

		[XmlArray("Rates")]
		[XmlArrayItem("Rate")]
		public List<RateDto> Rates { get; set; } = new List<RateDto>();
	}
}
