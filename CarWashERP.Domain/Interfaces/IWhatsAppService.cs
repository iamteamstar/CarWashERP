using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashERP.Domain.Interfaces
{
	public interface IWhatsAppService
	{
		Task<bool> SendMessageAsync(string phoneNumber, string message);
	}
}
