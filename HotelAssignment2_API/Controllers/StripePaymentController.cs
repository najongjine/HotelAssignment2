﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAssignment2_API.Controllers
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class StripePaymentController : Controller
  {
    private readonly IConfiguration _configuration;

    public StripePaymentController(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Create(StripePaymentDTO payment)
    {
      try
      {
        var domain = _configuration.GetValue<string>("HiddenVilla_Client_URL");
        var option = new SessionCreateOptions
        {
          PaymentMethodTypes=new List<string>
          {
            "card"
          }
          ,LineItems=new List<SessionLineItemOptions>
          {
            new SessionLineItemOptions
            {
              PriceData=new SessionLineItemPriceDataOptions
              {
                UnitAmount=payment.Amount
                ,Currency="USD"
                ,ProductData=new SessionLineItemPriceDataProductDataOptions
                {
                  Name=payment.ProductName
                }
              }
              ,Quantity=1
            }
          }
          ,Mode="payment"
          ,SuccessUrl=domain+"/success-payment?session_id={{CHECKOUT_SESSION_ID}}" //stripe return session id. automacally populates {{}} field
          ,CancelUrl=domain+payment.ReturnUrl
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(option);

        return Ok(new SuccessModel()
        {
          Data=session.Id
        });
      }catch(Exception e)
      {
        return BadRequest(new ErrorModel() { ErrorMessage=e.Message});
      }
    }

  }
}
