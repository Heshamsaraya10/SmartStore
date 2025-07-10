using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Domain.Entities
{
    public class Store
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public string? CommercialRegistrationNumber { get; set; }
        public string? TaxRegistrationNumber { get; set; }
        public string? TaxCardNumber { get; set; }
        public string? LicenseNumber { get; set; }
        public string? LegalName { get; set; }
        public string? BusinessType { get; set; }
        public DateTime? EstablishmentDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<StoreItemQuantity> StoreItemQuantities { get; set; } = new List<StoreItemQuantity>();
        public ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();
        public ICollection<DamagedItem> DamagedItems { get; set; } = new List<DamagedItem>();
    }
}
