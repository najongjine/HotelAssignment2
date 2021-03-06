using Business.Repository;
using Business.Repository.IRepository;
using DataAccess.Data;
using HotelAssignment2_API.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAssignment2_API
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
    public void ConfigureServices(IServiceCollection services)
    {
      Console.WriteLine("## ConfigureServices");
      Console.WriteLine("## ConfigureServices env: "+ _Env);
      // dependency container에 넣는 작업
      services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
      services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

      //클라에서 API 요청 보낼시, 보안에서 체크할 옵션값을 json 파일에서 읽어오는 코드. secret key 작업
      
      var appSettingSection = Configuration.GetSection("APISettings");
      if (_Env.ToLower() == "Development".ToLower())
      {
        appSettingSection = Configuration.GetSection("LocalAPISettings");
      }
      
      /* appsetting.json /  APISettings 밑에 있는 키 네임과 APISettings 객체 안에 있는 동일한 이름의 프로퍼티를 자동으로 맵핑
       dependency injection 기능도 같이 있다*/
      services.Configure<APISettings>(appSettingSection);

      services.Configure<MailJetSettings>(Configuration.GetSection("MailJetSettings"));

      var apiSettings = appSettingSection.Get<APISettings>();
      var key = Encoding.UTF8.GetBytes(apiSettings.SecretKey);

      services.AddAuthentication(opt =>
      {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuerSigningKey = true
          ,
          IssuerSigningKey = new SymmetricSecurityKey(key)
          ,
          ValidateAudience = true
          ,
          ValidateIssuer = true
          ,
          ValidAudience = apiSettings.ValidAudience
          ,
          ValidIssuer = apiSettings.ValidIssuer
          ,
          ClockSkew = TimeSpan.Zero
        };
      });

      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      services.AddScoped<IHotelRoomRepository, HotelRoomRepository>();
      services.AddScoped<IHotelImageRepository, HotelImagesRepository>();
      services.AddScoped<IHotelAmenityRepository, HotelAmenityRepository>();
      services.AddScoped<IRoomOrderDetailsRepository, RoomOrderDetailsRepository>();
      services.AddScoped<IEmailSender, EmailSender>();

      services.AddCors(o => o.AddPolicy("HiddenVilla", builder =>
      {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
      }));
      services.AddRouting(option => option.LowercaseUrls = true);
      services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null)
        .AddNewtonsoftJson(opt =>
        {
          opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
          opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelAssignment2_API", Version = "v1" });

        /* basic configuration inside the Swagger to add bearer token */
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          In = ParameterLocation.Header,
          Description = "Please Bearer and then token in the field",
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      Console.WriteLine("## Configure");
      StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["ApiKey"];
      app.UseSwagger();
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwaggerUI(c =>
        {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelAssignment2_API v1");
        });
      }
      else
      {
        app.UseSwaggerUI(c =>
        {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelAssignment2_API v1");
          c.RoutePrefix = string.Empty;
        });
      }
      app.UseHttpsRedirection();
      app.UseCors("HiddenVilla");
      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
