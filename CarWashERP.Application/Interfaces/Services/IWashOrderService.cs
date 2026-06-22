using CarWashERP.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashERP.Application.Interfaces.Services
{
	public interface IWashOrderService
	{
		// JWT Token'dan gelen tenantId'yi de parametre olarak alıyoruz
		Task<bool> CreateOrderAsync(CreateWashOrderDto dto, int tenantId);
	}
}
