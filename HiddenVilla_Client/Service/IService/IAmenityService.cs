using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla_Client.Service.IService
{
  public interface IAmenityService
  {
    public Task<IEnumerable<HotelAmenityDTO>> GetAllAmenities();
  }
}
