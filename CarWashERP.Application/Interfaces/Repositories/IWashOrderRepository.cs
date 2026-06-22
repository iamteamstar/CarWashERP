using CarWashERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashERP.Application.Interfaces.Repositories
{
	public interface IWashOrderRepository : IGenericRepository<WashOrder>
	{
		// İlişkili tablolarla (Paket, Çalışanlar) birlikte siparişi getiren özel metod
		Task<WashOrder?> GetWashOrderWithDetailsAsync(int id);
	}
}
