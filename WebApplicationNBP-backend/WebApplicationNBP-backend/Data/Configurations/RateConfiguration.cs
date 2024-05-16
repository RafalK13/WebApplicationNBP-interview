using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplicationNBP_backend.Domain;

namespace WebApplicationNBP_backend.Data.Configurations
{
	public class RateConfiguration : IEntityTypeConfiguration<Rate>
	{
		public void Configure(EntityTypeBuilder<Rate> builder)
		{
			builder.HasKey(r => r.Id);
			builder.Property(r => r.Id).ValueGeneratedOnAdd();
			builder.Property(r => r.MidRate).IsRequired().HasColumnType("decimal(18,4)");

			builder.HasOne(r => r.Currency)
				   .WithMany(c => c.Rates)
				   .HasForeignKey(r => r.CurrencyId);

			builder.HasOne(r => r.ExchangeRatesTable)
				   .WithMany(ert => ert.Rates)
				   .HasForeignKey(r => r.ExchangeRatesTableId);
		}
	}
}
