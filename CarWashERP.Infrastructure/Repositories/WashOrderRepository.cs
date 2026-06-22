using CarWashERP.Application.Interfaces.Repositories;
using CarWashERP.Domain.Entities;
using CarWashERP.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarWashERP.Infrastructure.Repositories;

public class WashOrderRepository : GenericRepository<WashOrder>, IWashOrderRepository
{
	public WashOrderRepository(ApplicationDbContext context) : base(context)
	{
	}

	public async Task<WashOrder?> GetWashOrderWithDetailsAsync(int id)
	{
		return await _context.WashOrders
			.Include(w => w.ServicePackage)
			.Include(w => w.WashOrderEmployees)
				.ThenInclude(we => we.Employee)
			.FirstOrDefaultAsync(w => w.Id == id);
	}
}