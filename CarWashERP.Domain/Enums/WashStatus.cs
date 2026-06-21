using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashERP.Domain.Enums
{
	public enum WashStatus
	{
		Pending = 1,       // Araç teslim alındı, sırada
		InProgress = 2,    // Yıkama aşamasında
		Finishing = 3,     // Son dokunuşlar / 5 dk içinde hazır
		Completed = 4,     // Teslim edildi / Tamamlandı
		Cancelled = 5      // İptal edildi
	}

	public enum PackageType
	{
		OnlyExterior = 1,  // Sadece Dış
		InteriorAndExterior = 2 // İç - Dış
	}
}
