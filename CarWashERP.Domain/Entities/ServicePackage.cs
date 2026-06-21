using CarWashERP.Domain.Common;
using CarWashERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashERP.Domain.Entities
{
	public class ServicePackage : BaseEntity, IMustHaveTenant
	{
		public int TenantId { get; set; }
		public string Name { get; set; } = string.Empty; // Temel, Orta, Premium, Delux vb.
		public string Description { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public PackageType Type { get; set; }

		// Navigation Properties
		public Tenant Tenant { get; set; } = null!;
		public ICollection<WashOrder> WashOrders { get; set; } = new HashSet<WashOrder>();
	}
}
