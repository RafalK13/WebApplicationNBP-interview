using System.Xml.Serialization;

namespace WebApplicationNBP_backend.Domain.DTOs
{
	[XmlRoot("ArrayOfExchangeRatesTable")]
	public class ArrayOfExchangeRatesTableDto
	{
		[XmlElement("ExchangeRatesTable")]
		public List<ExchangeRatesTableDto> ExchangeRatesTable { get; set; } = new List<ExchangeRatesTableDto>();
	}
}