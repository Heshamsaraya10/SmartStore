using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Domain.Dtos.Request
{
    public class StockAdjustmentRequestDto
    {
        public int StoreId { get; set; }
        public int ItemId { get; set; }
        public decimal QuantityBefore { get; set; }
        public decimal QuantityAfter { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string? Reason { get; set; }
        public bool? IsActive { get; set; }
    }
}
