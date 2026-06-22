using CarWashERP.Application.DTOs;
using CarWashERP.Application.Interfaces.Repositories;
using CarWashERP.Application.Interfaces.Services;
using CarWashERP.Domain.Entities;

namespace CarWashERP.Application.Services;

public class WashOrderService : IWashOrderService
{
	private readonly IWashOrderRepository _washOrderRepository;
	private readonly IGenericRepository<ServicePackage> _packageRepository;
	private readonly IGenericRepository<Tenant> _tenantRepository;

	public WashOrderService(
		IWashOrderRepository washOrderRepository,
		IGenericRepository<ServicePackage> packageRepository,
		IGenericRepository<Tenant> tenantRepository)
	{
		_washOrderRepository = washOrderRepository;
		_packageRepository = packageRepository;
		_tenantRepository = tenantRepository;
	}

	public async Task<bool> CreateOrderAsync(CreateWashOrderDto dto, int tenantId)
	{
		// 1. Paket ve İşletme bilgilerini getir (Fiyat ve Prim oranı için)
		var package = await _packageRepository.GetByIdAsync(dto.ServicePackageId);
		var tenant = await _tenantRepository.GetByIdAsync(tenantId);

		if (package == null || tenant == null) return false;

		// 2. Sipariş Entity'sini oluştur ve o anki değerleri Snapshot olarak kaydet
		var washOrder = new WashOrder
		{
			TenantId = tenantId,
			Plate = dto.Plate,
			CustomerName = dto.CustomerName,
			CustomerPhone = dto.CustomerPhone,
			ServicePackageId = dto.ServicePackageId,
			SnapshottedPrice = package.Price, // O anki paket fiyatı
			SnapshottedCommissionRate = tenant.DefaultCommissionRate // O anki işletme prim oranı
		};

		// 3. Çoklu Çalışan Prim Bölüştürme Mantığı
		if (dto.EmployeeIds != null && dto.EmployeeIds.Any())
		{
			int employeeCount = dto.EmployeeIds.Count;
			// Örneğin 2 kişi ise ShareRatio 0.50 (Yarım) olur.
			decimal shareRatio = 1.0m / employeeCount;

			// Toplam prim tutarı (Örn: 1000 TL * %30 = 300 TL)
			decimal totalCommissionAmount = washOrder.SnapshottedPrice * (washOrder.SnapshottedCommissionRate / 100);

			foreach (var empId in dto.EmployeeIds)
			{
				washOrder.WashOrderEmployees.Add(new WashOrderEmployee
				{
					EmployeeId = empId,
					ShareRatio = shareRatio,
					// Kişi başı düşen net prim (Örn: 300 TL * 0.50 = 150 TL)
					EarnedAmount = totalCommissionAmount * shareRatio
				});
			}
		}

		// 4. Veritabanına kaydet
		await _washOrderRepository.AddAsync(washOrder);
		// Not: SaveChangesAsync işlemi normalde UnitOfWork veya Repository içinde çağrılır.

		// 5. WhatsApp Mesajını Tetikle (Şimdilik mock interface çağrılabilir)
		// _whatsAppService.SendMessageAsync(...)

		return true;
	}
}