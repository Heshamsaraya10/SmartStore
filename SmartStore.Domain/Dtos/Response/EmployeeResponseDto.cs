using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Domain.Dtos.Response
{
    public record EmployeeResponseDto
    {
        public int EmployeeId { get; set; }
        public string NameArabic { get; set; }
    }
}
