using Microsoft.EntityFrameworkCore;

namespace GeoCam.Api.DataAccess
{
	public class GeoCamDbContext : DbContext
	{
		public GeoCamDbContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Camera>()
				.Property(p => p.Id)
				.ValueGeneratedOnAdd();
			// Camera names aren't unique but the combination of long/lat should be
			modelBuilder.Entity<Camera>()
				.HasIndex(c => c.Name);
			modelBuilder.Entity<Camera>()
				.HasIndex(c => new { c.Longitude, c.Latitude })
				.IsUnique();
		}

		#region Properties

		public DbSet<Camera> Cameras { get; set; }

		#endregion
	}
}
