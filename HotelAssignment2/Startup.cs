using DataAccess.Data;
using HotelAssignment2.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Repository.IRepository;
using Business.Repository;
using HotelAssignment2.Service.IService;
using HotelAssignment2.Service;
using Microsoft.AspNetCore.Identity;

namespace HotelAssignment2
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
      Configuration = configuration;
      _Env = env.EnvironmentName;
    }

    public static string _Env { get; set; }
    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      System.Diagnostics.Debug.WriteLine("## System.Diagnostics.Debug.WriteLine");
      System.Diagnostics.Trace.TraceError("## System.Diagnostics.Trace.TraceError");
      Console.WriteLine("## _Env in server: " + _Env);
      // dependency container에 넣는 작업
      services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
      services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders().AddDefaultUI();
      
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      services.AddScoped<IHotelRoomRepository, HotelRoomRepository>();
      services.AddScoped<IHotelImageRepository, HotelImagesRepository>();
      services.AddScoped<IDbInitializer, DbInitializer>();
      services.AddScoped<IFileUpload, FileUpload>();
      services.AddScoped<IHotelAmenityRepository, HotelAmenityRepository>();
      services.AddScoped<IRoomOrderDetailsRepository, RoomOrderDetailsRepository>();
      services.AddRazorPages();
      services.AddHttpContextAccessor();
      services.AddServerSideBlazor();
      services.AddSingleton<WeatherForecastService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)//
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      dbInitializer.Initialize();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapRazorPages();
        endpoints.MapBlazorHub();
        endpoints.MapFallbackToPage("/_Host");
      });
    }
  }
}
