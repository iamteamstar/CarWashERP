using CarWashERP.Application.DTOs;
using CarWashERP.Application.Interfaces.Repositories;
using CarWashERP.Application.Interfaces.Services;
using CarWashERP.Domain.Entities;

namespace CarWashERP.Application.Services;

public class WashOrderService : IWashOrderService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IGenericRepository<ServicePackage> _packageRepository;
	private readonly IGenericRepository<Tenant> _tenantRepository;

	public WashOrderService(
		IUnitOfWork unitOfWork,
		IGenericRepository<ServicePackage> packageRepository,
		IGenericRepository<Tenant> tenantRepository)
	{
		_unitOfWork = unitOfWork;
		_packageRepository = packageRepository;
		_tenantRepository = tenantRepository;
	}

	public async Task<bool> CreateOrderAsync(CreateWashOrderDto dto, int tenantId)
	{
		var package = await _packageRepository.GetByIdAsync(dto.ServicePackageId);
		var tenant = await _tenantRepository.GetByIdAsync(tenantId);

		if (package == null || tenant == null) return false;

		var washOrder = new WashOrder
		{
			TenantId = tenantId,
			Plate = dto.Plate,
			CustomerName = dto.CustomerName,
			CustomerPhone = dto.CustomerPhone,
			ServicePackageId = dto.ServicePackageId,
			SnapshottedPrice = package.Price,
			SnapshottedCommissionRate = tenant.DefaultCommissionRate
		};

		if (dto.EmployeeIds != null && dto.EmployeeIds.Any())
		{
			int employeeCount = dto.EmployeeIds.Count;
			decimal shareRatio = 1.0m / employeeCount;
			decimal totalCommissionAmount = washOrder.SnapshottedPrice * (washOrder.SnapshottedCommissionRate / 100);

			foreach (var empId in dto.EmployeeIds)
			{
				washOrder.WashOrderEmployees.Add(new WashOrderEmployee
				{
					EmployeeId = empId,
					ShareRatio = shareRatio,
					EarnedAmount = totalCommissionAmount * shareRatio
				});
			}
		}

		// 1. Veriyi DbContext'in takibine (memory'ye) ekle
		await _unitOfWork.WashOrders.AddAsync(washOrder);

		// 2. Unit of Work ile tüm değişiklikleri tek bir transaction olarak veritabanına yansıt (Commit)
		var result = await _unitOfWork.SaveChangesAsync();

		if (result > 0)
		{
			// İşlem başarılıysa WhatsApp mesajını tetikle
			return true;
		}

		return false; // İşlem başarısız (Rollback)
	}
}