﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Domain.Dtos.Request
{
    public record EmployeeRequestDto
    {
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
        public string JobTitle { get; set; }
        public string Phone { get; set; }
    }
}
