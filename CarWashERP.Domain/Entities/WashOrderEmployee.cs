using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashERP.Domain.Entities
{
	public class WashOrderEmployee
	{
		public int WashOrderId { get; set; }
		public WashOrder WashOrder { get; set; } = null!;

		public int EmployeeId { get; set; }
		public Employee Employee { get; set; } = null!;

		// Havuzdaki primin kaçta kaçını aldığını tutar. 
		// Tek kişi yıkadıysa 1.00 (Tam), İki kişi yıkadıysa çalışan başına 0.50 (Yarım) düşer.
		public decimal ShareRatio { get; set; } = 1.00m;

		// O işlemden kazandığı net prim tutarı (SnapshottedPrice * (SnapshottedCommissionRate/100) * ShareRatio)
		public decimal EarnedAmount { get; set; }
	}
}
