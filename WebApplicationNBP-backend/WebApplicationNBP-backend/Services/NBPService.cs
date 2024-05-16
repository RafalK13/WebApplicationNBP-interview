using System.Xml.Serialization;
using WebApplicationNBP_backend.Domain.DTOs;
using WebApplicationNBP_backend.Interfaces;

namespace WebApplicationNBP_backend.Services
{

	public class NBPService : INBPService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger _logger;

		public NBPService(IHttpClientFactory httpClientFactory, ILogger<NBPService> logger)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
		}

		public async Task<ExchangeRatesTableDto> GetExchangeRatesFromTable(string tableName)
		{
			var content = await ExecuteRequestToNBP(tableName);

			var arrayOfExchangeRateDto = SerializeContent<ArrayOfExchangeRatesTableDto>(content);

			return arrayOfExchangeRateDto.ExchangeRatesTable.FirstOrDefault();
		}

		private T SerializeContent<T>(string content)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));

			using (var reader = new StringReader(content))
			{
				return (T)serializer.Deserialize(reader);
			}
		}

		private async Task<string> ExecuteRequestToNBP(string tableName, CancellationToken cancellationToken = default)
		{
			var httpClient = _httpClientFactory.CreateClient("HttpClientNBP");

			try
			{
				string url = $"{tableName}";
				var response = await httpClient.GetAsync(url);

				if (!response.IsSuccessStatusCode)
				{
					_logger
					.LogError("NBPService.ExecuteRequestToNBP | Error during sending GetAsync. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}",
					response.StatusCode,
					response.ReasonPhrase);
					return null;
				}
				var content = await response.Content.ReadAsStringAsync(cancellationToken);

				if (content == null)
				{
					_logger.LogError("NBPService.ExecuteRequestToNBP | Error during ReadAsStreamAsync. Conent is null.");
					return null;
				}
				return content;

			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(httpEx, "NBPService.{methodName}. NBP application is not reachable.", nameof(ExecuteRequestToNBP));
				throw new HttpRequestException("NBP application is not reachable", httpEx, System.Net.HttpStatusCode.ServiceUnavailable);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "NBPService.{methodName}", nameof(ExecuteRequestToNBP));
			}
			return null;
		}
	}
}
