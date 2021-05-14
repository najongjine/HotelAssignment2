using HiddenVilla_Client.Service.IService;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HiddenVilla_Client.Service
{
  public class AmenityService : IAmenityService
  {
    private readonly HttpClient _client;

    public AmenityService(HttpClient client)
    {
      _client = client;
    }

    public async Task<IEnumerable<HotelAmenityDTO>> GetAllAmenities()
    {
      var response=await _client.GetAsync($"api/Amenity");
      var content =await response.Content.ReadAsStringAsync();

      // Enitity to DTO convert is alrdy done in repository
      var amenities = JsonConvert.DeserializeObject<IEnumerable<HotelAmenityDTO>>(content);
      return amenities;
    }
  }
}
