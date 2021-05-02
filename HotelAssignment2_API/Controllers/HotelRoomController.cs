using Business.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAssignment2_API.Controllers
{
  [Route("api/[controller]")]
  public class HotelRoomController : Controller
  {
    private readonly IHotelRoomRepository _hotelRoomRepository;

    public HotelRoomController(IHotelRoomRepository hotelRoomRepository)
    {
      _hotelRoomRepository = hotelRoomRepository;
    }


    [HttpGet]
    public async Task<IActionResult> GetHotelRooms()
    {
      var allRooms = await _hotelRoomRepository.GetAllHotelRoom();
      return Ok(allRooms);
    }
  }
}
