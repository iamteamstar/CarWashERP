using CarWashERP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashERP.Domain.Entities
{
	public class Tenant : BaseEntity
	{
		public string Name { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty; // Resmi ticari unvan
		public string Phone { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public bool IsActive { get; set; } = true;

		// İşletmeye özel varsayılan prim oranı (örn: 30)
		public decimal DefaultCommissionRate { get; set; } = 30.00m;

		// Navigation Properties
		public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
		public ICollection<ServicePackage> ServicePackages { get; set; } = new HashSet<ServicePackage>();
		public ICollection<WashOrder> WashOrders { get; set; } = new HashSet<WashOrder>();
		public ICollection<Expense> Expenses { get; set; } = new HashSet<Expense>();
	}
}
