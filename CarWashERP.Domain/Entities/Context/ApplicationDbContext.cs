using CarWashERP.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CarWashERP.Domain.Entities.Context;

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

			// 1. Composite Key Ayarı (Many-to-Many tablosu için)
			modelBuilder.Entity<WashOrderEmployee>()
				.HasKey(we => new { we.WashOrderId, we.EmployeeId });

			// 2. Multi-Tenant Global Query Filters
			// Her sorgunun sonuna otomatik olarak "WHERE TenantId = _currentTenantId" ekler.
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

