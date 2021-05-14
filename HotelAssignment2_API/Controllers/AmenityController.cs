using Business.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAssignment2_API.Controllers
{
  [Route("api/[controller]")]
  public class AmenityController : Controller
  {
    private readonly IHotelAmenityRepository _amenityRepository;
    public AmenityController(IHotelAmenityRepository amenityRepository)
    {
      _amenityRepository = amenityRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAmenities()
    {
      //DTO 로 변환은 repository 에서 다 끝났음
      var amenities=await _amenityRepository.GetAllAmenities();
      return Ok(amenities);
    }
  }
}
