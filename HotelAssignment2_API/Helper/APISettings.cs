﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAssignment2_API.Helper
{
  public class APISettings
  {
    public string SecretKey { get; set; }
    public string ValidAudience { get; set; }
    public string ValidIssuer { get; set; }
  }
}
