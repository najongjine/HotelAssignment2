using Business.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public async Task<IActionResult> GetHotelRooms(string checkInDate = null, string checkOutDate = null)
    {
      if (string.IsNullOrWhiteSpace(checkInDate) || string.IsNullOrWhiteSpace(checkOutDate))
      {
        return BadRequest(new ErrorModel() { 
          StatusCode=StatusCodes.Status400BadRequest,
          ErrorMessage="All Parameters need to be supplied"
        });
      }
      
      if (!DateTime.TryParseExact(checkInDate,"MM'/'dd'/'yyyy",CultureInfo.InvariantCulture,DateTimeStyles.None,out var dtCheckInDate) )
      {
        return BadRequest(new ErrorModel()
        {
          StatusCode = StatusCodes.Status400BadRequest,
          ErrorMessage = "invalid checkin date format. valid format will be MM/dd/yyyy"
        });
      }
      if (!DateTime.TryParseExact(checkOutDate, "MM'/'dd'/'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtCheckOutDate))
      {
        return BadRequest(new ErrorModel()
        {
          StatusCode = StatusCodes.Status400BadRequest,
          ErrorMessage = "invalid checkout date format. valid format will be MM/dd/yyyy"
        });
      }
      
      // Enitity to DTO convert is alrdy done in repository
      var allRooms = await _hotelRoomRepository.GetAllHotelRoom(checkInDate,checkOutDate);
      return Ok(allRooms);
    }

    [HttpGet("{roomId}")]
    public async Task<IActionResult> GetHotelRoom(int? roomId, string checkInDate = null, string checkOutDate = null)
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
      if (string.IsNullOrWhiteSpace(checkInDate) || string.IsNullOrWhiteSpace(checkOutDate))
      {
        return BadRequest(new ErrorModel()
        {
          StatusCode = StatusCodes.Status400BadRequest,
          ErrorMessage = "All Parameters need to be supplied"
        });
      }
      if (!DateTime.TryParseExact(checkInDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtCheckInDate))
      {
        return BadRequest(new ErrorModel()
        {
          StatusCode = StatusCodes.Status400BadRequest,
          ErrorMessage = "invalid checkin date format. valid format will be MM/dd/yyyy"
        });
      }
      if (!DateTime.TryParseExact(checkOutDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtCheckOutDate))
      {
        return BadRequest(new ErrorModel()
        {
          StatusCode = StatusCodes.Status400BadRequest,
          ErrorMessage = "invalid checkout date format. valid format will be MM/dd/yyyy"
        });
      }

      var roomDetails = await _hotelRoomRepository.GetHotelRoom(roomId.Value,checkInDate,checkOutDate);
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
