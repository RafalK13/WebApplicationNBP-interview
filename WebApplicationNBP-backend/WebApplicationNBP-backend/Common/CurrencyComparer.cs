using WebApplicationNBP_backend.Domain;

namespace WebApplicationNBP_backend.Common
{
	public class CurrencyComparer : IEqualityComparer<Currency>
	{
		public bool Equals(Currency x, Currency y)
		{
			if (x == null || y == null) return false;
			return x.Name == y.Name && x.Code == y.Code;
		}

		public int GetHashCode(Currency obj)
		{
			if (obj == null) return 0;
			return HashCode.Combine(obj.Name, obj.Code);
		}
	}
}
