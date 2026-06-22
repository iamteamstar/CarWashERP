using CarWashERP.Application.Interfaces.Repositories;
using CarWashERP.Infrastructure.Context;

namespace CarWashERP.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _context;

	// Repository'lerin sadece ihtiyaç duyulduğunda (Lazy Loading mantığıyla) 
	// oluşturulması performansı artırır.
	private IWashOrderRepository? _washOrders;

	public UnitOfWork(ApplicationDbContext context)
	{
		_context = context;
	}

	public IWashOrderRepository WashOrders =>
		_washOrders ??= new WashOrderRepository(_context);

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		// EF Core'un SaveChanges metodu zaten varsayılan olarak bir Transaction içinde çalışır.
		return await _context.SaveChangesAsync(cancellationToken);
	}

	public void Dispose()
	{
		_context.Dispose();
		GC.SuppressFinalize(this);
	}
}