using Business.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
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

    [HttpGet("{roomId}")]
    public async Task<IActionResult> GetHotelRoom(int? roomId)
    {
      if (roomId == null)
      {
        // 여기서 쓴 ErrorModel은 Model project에 정의해 놓은 custom class
        return BadRequest(new ErrorModel()
        {
          Title = "",
          ErrorMessage = "Invalid Room Id",
          StatusCode = StatusCodes.Status400BadRequest
        });
      }

      var roomDetails = await _hotelRoomRepository.GetHotelRoom(roomId.Value);
      if (roomDetails == null)
      {
        // 여기서 쓴 ErrorModel은 Model project에 정의해 놓은 custom class
        return BadRequest(new ErrorModel()
        {
          Title = "",
          ErrorMessage = "Invalid Room Id",
          StatusCode = StatusCodes.Status404NotFound
        });
      }
      return Ok(roomDetails);
    }
  }
}
