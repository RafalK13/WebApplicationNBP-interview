using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplicationNBP_backend.Domain;

namespace WebApplicationNBP_backend.Data.Configurations
{
	public class ExchangeRatesTableConfiguration : IEntityTypeConfiguration<ExchangeRatesTable>
	{
		public void Configure(EntityTypeBuilder<ExchangeRatesTable> builder)
		{
			builder.HasKey(ert => ert.Id);
			builder.Property(ert => ert.EffectiveDate).IsRequired();
		}
	}
}
