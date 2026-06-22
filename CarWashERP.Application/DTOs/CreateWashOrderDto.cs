using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashERP.Application.DTOs
{
	public class CreateWashOrderDto
	{
		public string Plate { get; set; } = string.Empty;
		public string CustomerName { get; set; } = string.Empty;
		public string CustomerPhone { get; set; } = string.Empty;
		public int ServicePackageId { get; set; }

		// İşletme aracı sisteme girerken bir veya birden fazla çalışan seçebilir
		public List<int> EmployeeIds { get; set; } = new List<int>();
	}
}
