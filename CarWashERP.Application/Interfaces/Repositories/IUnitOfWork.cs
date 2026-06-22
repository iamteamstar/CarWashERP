namespace CarWashERP.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
	IWashOrderRepository WashOrders { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}