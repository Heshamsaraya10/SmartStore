﻿using SmartStore.Domain.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Domain.Dtos.Response
{
    public class CustomerResponseDto : CustomerRequestDto
    {
        public int CustomerId { get; set; }
    }
}
