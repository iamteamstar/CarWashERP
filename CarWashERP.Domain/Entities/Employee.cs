using CarWashERP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashERP.Domain.Entities
{
	public class Employee : BaseEntity, IMustHaveTenant
	{
		public int TenantId { get; set; }
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
		public decimal BaseSalary { get; set; } // Personelin sabit maaşı
		public bool IsActive { get; set; } = true;

		// Navigation Properties
		public Tenant Tenant { get; set; } = null!;
		public ICollection<WashOrderEmployee> WashOrderEmployees { get; set; } = new HashSet<WashOrderEmployee>();
	}
}
