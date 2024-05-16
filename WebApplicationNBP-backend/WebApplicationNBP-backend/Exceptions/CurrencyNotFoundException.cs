namespace WebApplicationNBP_backend.Exceptions
{
	public class CurrencyNotFoundException : Exception
	{
		public CurrencyNotFoundException(string code)
			: base($"Currency code {code} not found in cache.")
		{
		}
	}
}
