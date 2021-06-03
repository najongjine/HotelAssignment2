using Business.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Models;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAssignment2_API.Controllers
{
  [ApiController]
  [Route("api/[controller]/[action]")]
  public class RoomOrderController : Controller
  {
    private readonly IRoomOrderDetailsRepository _repository;
    private readonly IEmailSender _emailSender;

    public RoomOrderController(IRoomOrderDetailsRepository repository, IEmailSender emailSender)
    {
      _repository = repository;
      _emailSender = emailSender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoomOrderDetailsDTO details)
    {
      try
      {
        if (ModelState.IsValid)
        {
          var result = await _repository.Create(details);
          return Ok(result);
        }
        else
        {
          return BadRequest(new ErrorModel() { ErrorMessage = "Error while creating Room Details/Booking" });
        }
      }
      catch (Exception e)
      {
        return BadRequest(new ErrorModel() { ErrorMessage = e.Message });
      }
    }
    [HttpPost]
    public async Task<IActionResult> PaymentSuccessful([FromBody] RoomOrderDetailsDTO details)
    {
      try
      {
        var service = new SessionService();
        //Stripe 사이트에서 sessionId로 세부정보 가져오는 코드
        var sessionDetails = service.Get(details.StripeSessionId);
        if (sessionDetails.PaymentStatus == "paid")
        {
          var result = await _repository.MarkPaymentSuccessful(details.Id);
          if (result == null)
          {
            return BadRequest(new ErrorModel() { ErrorMessage = "Cannot mark payment as successful" });
          }
          await _emailSender.SendEmailAsync(details.Email,"Hidden villa payment msg",$"Your booking has been confirmed at HiddenVilla " +
            $" with order ID : {details.Id}");
          return Ok(result);
        }
        else
        {
          return BadRequest(new ErrorModel() { ErrorMessage = $"{sessionDetails.PaymentStatus}. Cannot mark payment as successful" });
        }
      }
      catch (Exception e)
      {
        return BadRequest(new ErrorModel() { ErrorMessage = e.Message });
      }
    }

  }
}
