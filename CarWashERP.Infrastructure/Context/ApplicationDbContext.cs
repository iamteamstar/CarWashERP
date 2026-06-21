using CarWashERP.Domain.Common;
using CarWashERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarWashERP.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
	// Gerçek senaryoda bu ID, API katmanından (JWT Token içinden) okunarak enjekte edilecek.
	// Şimdilik test için varsayılan bir değer atıyoruz.
	private readonly int _currentTenantId = 1;

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}

	public DbSet<Tenant> Tenants { get; set; }
	public DbSet<Employee> Employees { get; set; }
	public DbSet<ServicePackage> ServicePackages { get; set; }
	public DbSet<WashOrder> WashOrders { get; set; }
	public DbSet<WashOrderEmployee> WashOrderEmployees { get; set; }
	public DbSet<Expense> Expenses { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<WashOrderEmployee>()
			.HasKey(we => new { we.WashOrderId, we.EmployeeId });

		modelBuilder.Entity<Employee>().HasQueryFilter(e => e.TenantId == _currentTenantId);
		modelBuilder.Entity<ServicePackage>().HasQueryFilter(e => e.TenantId == _currentTenantId);
		modelBuilder.Entity<WashOrder>().HasQueryFilter(e => e.TenantId == _currentTenantId);
		modelBuilder.Entity<Expense>().HasQueryFilter(e => e.TenantId == _currentTenantId);

		// Ekstra: Delete davranışını Restrict yaparak kazara ilişkili veri silinmesini engelleyebiliriz
		foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
		{
			relationship.DeleteBehavior = DeleteBehavior.Restrict;
		}
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		// 3. BaseEntity tarih otomasyonu
		var entries = ChangeTracker.Entries<BaseEntity>();

		foreach (var entry in entries)
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedDate = DateTime.UtcNow;
					break;
				case EntityState.Modified:
					entry.Entity.UpdatedDate = DateTime.UtcNow;
					break;
			}
		}

		return base.SaveChangesAsync(cancellationToken);
	}
}