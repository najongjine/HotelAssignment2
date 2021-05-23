using HiddenVilla_Client.Service.IService;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HiddenVilla_Client.Service
{
  public class StripePaymentService : IStripePaymentService
  {
    private readonly HttpClient _client;
    public StripePaymentService(HttpClient client)
    {
      _client = client;
    }
    public async Task<SuccessModel> CheckOut(StripePaymentDTO model)
    {
      try
      {
        var content = JsonConvert.SerializeObject(model);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("api/StripePayment/Create",bodyContent);
        if (response.IsSuccessStatusCode)
        {
          var contentTemp = await response.Content.ReadAsStringAsync();
          // Enitity to DTO convert is alrdy done in repository
          var result = JsonConvert.DeserializeObject<SuccessModel>(contentTemp);
          return result;
        }
        else
        {
          var contentTemp = await response.Content.ReadAsStringAsync();
          // Enitity to DTO convert is alrdy done in repository
          var errorModel = JsonConvert.DeserializeObject<ErrorModel>(contentTemp);
          throw new Exception(errorModel.ErrorMessage);
        }
      }
      catch(Exception e)
      {
        throw new Exception(e.Message);
      }
    }
  }
}
