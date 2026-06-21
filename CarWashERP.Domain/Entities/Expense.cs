	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarWashERP.Domain.Common;
namespace CarWashERP.Domain.Entities
{
	public class Expense : BaseEntity, IMustHaveTenant
	{
		public int TenantId { get; set; }
		public string Title { get; set; } = string.Empty; // Örn: Deterjan Alımı, Elektrik Faturası
		public string Description { get; set; } = string.Empty;
		public decimal Amount { get; set; }
		public DateTime ExpenseDate { get; set; }

		// Navigation Properties
		public Tenant Tenant { get; set; } = null!;
	}
}
