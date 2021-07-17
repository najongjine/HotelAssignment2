using Business.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAssignment2_API.Controllers
{
  [Route("api/[controller]")]
  public class TestController : Controller
  {
    private readonly ILogger _logger;
    public TestController(ILogger<TestController> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Test()
    {
      _logger.LogInformation("## _logger.LogInformation test");
      return Ok("test");
    }
  }
}
