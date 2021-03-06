using Blazored.LocalStorage;
using HiddenVilla_Client.Service;
using HiddenVilla_Client.Service.IService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HiddenVilla_Client
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var builder = WebAssemblyHostBuilder.CreateDefault(args);
      builder.RootComponents.Add<App>("#app");

      // adding HttpClient injection. base url을 HotelAssgienment2_API 의 url 주소로 설정했음
      Console.WriteLine("## builder.HostEnvironment.Environment: " + builder.HostEnvironment.Environment);
      if(builder.HostEnvironment.Environment.ToLower() == "Development".ToLower()) {
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("LocalBaseApiUrl")) });
      }
      else
      {
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseApiUrl")) });
      }
      
      builder.Services.AddBlazoredLocalStorage();
      builder.Services.AddAuthorizationCore();
      builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
      builder.Services.AddScoped<IHotelRoomService, HotelRoomService>();
      builder.Services.AddScoped<IAmenityService, AmenityService>();
      builder.Services.AddScoped<IRoomOrderDetailsService, RoomOrderDetailsService>();
      builder.Services.AddScoped<IStripePaymentService, StripePaymentService>();
      builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
      await builder.Build().RunAsync();
    }
  }
}
