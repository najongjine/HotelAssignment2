using Common;
using DataAccess.Data;
using HotelAssignment2.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAssignment2.Service
{
  public class DbInitializer : IDbInitializer
  {
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      _db = db;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    public void Initialize()
    {
      try
      {
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
          _db.Database.Migrate();
        }
      }
      catch(Exception ex)
      {

      }

      // role table에 admin 이란게 있으면 role을 만들지 말고 빠져 나와라
      if (_db.Roles.Any(x => x.Name == SD.Role_Admin)) return;
      _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
      _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
      _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();

      // 뒤쪽 문자열은 비밀번호임
      _userManager.CreateAsync(new IdentityUser
      {
        UserName = "najongjine",
        Email = "najongjin3@hotmail.com",
        EmailConfirmed = true
      },"1q2w3e4r%T").GetAwaiter().GetResult();

      var user = _db.Users.FirstOrDefault(u => u.Email == "najongjin3@hotmail.com");
      _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
    }
  }
}
