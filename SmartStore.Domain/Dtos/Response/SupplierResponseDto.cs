using SmartStore.Domain.Dtos.Request;
using SmartStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Domain.Dtos.Response
{
    public class SupplierResponseDto : SupplierRequestDto
    {
        public int SupplierId { get; set; }

    }
}
