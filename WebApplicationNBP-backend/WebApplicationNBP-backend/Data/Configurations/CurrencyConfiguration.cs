using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplicationNBP_backend.Domain;

namespace WebApplicationNBP_backend.Data.Configurations
{
	public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
	{
		public void Configure(EntityTypeBuilder<Currency> builder)
		{
			builder.HasKey(c => c.Id);
			builder.Property(c => c.Code).IsRequired();
			builder.Property(c => c.Name).IsRequired();
		}
	}
}
