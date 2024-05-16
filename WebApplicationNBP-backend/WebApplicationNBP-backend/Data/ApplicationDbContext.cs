using Microsoft.EntityFrameworkCore;
using WebApplicationNBP_backend.Domain;

namespace WebApplicationNBP_backend.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Currency> Currencies { get; set; }
		public DbSet<ExchangeRatesTable> ExchangeRatesTables { get; set; }
		public DbSet<Rate> Rates { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
			base.OnModelCreating(modelBuilder);
		}
	}
}
