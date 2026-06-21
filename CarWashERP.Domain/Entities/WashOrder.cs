using CarWashERP.Domain.Common;
using CarWashERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashERP.Domain.Entities
{
	public class WashOrder : BaseEntity, IMustHaveTenant
	{
		public int TenantId { get; set; }
		public string Plate { get; set; } = string.Empty;
		public string CustomerName { get; set; } = string.Empty;
		public string CustomerPhone { get; set; } = string.Empty;

		public int ServicePackageId { get; set; }
		public decimal SnapshottedPrice { get; set; } // Sipariş anındaki paket fiyatı
		public decimal SnapshottedCommissionRate { get; set; } // Sipariş anındaki işletme prim oranı (%)

		public WashStatus Status { get; set; } = WashStatus.Pending;
		public DateTime? StatusUpdatedDate { get; set; }

		// Navigation Properties
		public Tenant Tenant { get; set; } = null!;
		public ServicePackage ServicePackage { get; set; } = null!;

		// Bir aracı birden fazla çalışan yıkayabilir (Many-to-Many)
		public ICollection<WashOrderEmployee> WashOrderEmployees { get; set; } = new HashSet<WashOrderEmployee>();
	}
}
