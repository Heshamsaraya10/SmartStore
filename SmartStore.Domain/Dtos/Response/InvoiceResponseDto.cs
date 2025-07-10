using SmartStore.Domain.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Domain.Dtos.Response
{
    public class InvoiceResponseDto : InvoiceRequestDto
    {
        public int InvoiceId { get; set; }
        public ICollection<InvoiceDetailResponseDto> Details { get; set; } = new List<InvoiceDetailResponseDto>();
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public object RemainingAmount { get; set; }
    }
}
