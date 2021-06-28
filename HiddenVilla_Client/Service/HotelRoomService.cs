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
  public class HotelRoomService : IHotelRoomService
  {
    private readonly HttpClient _client;

    public HotelRoomService( HttpClient client)
    {
      _client = client;
    }
    public async Task<HotelRoomDTO> GetHotelRoomDetail(int roomId, string checkInDate, string checkOutDate)
    {
      var response = await _client.GetAsync($"api/hotelroom/{roomId}?checkInDate={checkInDate}&checkOutDate={checkOutDate}");
      if (response.IsSuccessStatusCode)
      {
        var content = await response.Content.ReadAsStringAsync();
        // Enitity to DTO convert is alrdy done in repository
        var room = JsonConvert.DeserializeObject<HotelRoomDTO>(content);
        return room;
      }
      else
      {
        var content = await response.Content.ReadAsStringAsync();
        // Enitity to DTO convert is alrdy done in repository
        var errorModel = JsonConvert.DeserializeObject<ErrorModel>(content);
        throw new Exception(errorModel.ErrorMessage);
      }
    }

    public async Task<IEnumerable<HotelRoomDTO>> GetHotelRooms(string checkInDate, string checkOutDate)
    {
      //var response = await _client.GetAsync($"api/hotelroom?checkInDate={checkInDate}&checkOutDate={checkOutDate}");
      var response = await _client.GetAsync($"api/hotelroom");
      var content = await response.Content.ReadAsStringAsync();

      // Enitity to DTO convert is alrdy done in repository
      var rooms = JsonConvert.DeserializeObject<IEnumerable<HotelRoomDTO>>(content);
      return rooms;
    }
  }
}
