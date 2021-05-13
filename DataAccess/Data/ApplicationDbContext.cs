using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
  // IdentityDbContext<ApplicationUser> == DataAccess.Data 에 정의한 ApplicationUser class를 회원 테이블로 쓰겠다란 뜻.
  // IdentityDbContext 이렇게 하면 default 로 Microsoft.Identity.EntityFramework 에 있는 IdentityUser 객체가 자동으로 사용됨
  public class ApplicationDbContext :IdentityDbContext<IdentityUser>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {}
    public DbSet<HotelRoom> HotelRooms { get; set; }
    public DbSet<HotelRoomImage> HotelRoomsImages { get; set; }
    public DbSet<HotelAmenity> HotelAmenities { get; set; }
    public DbSet<ApplicationUser> ApplicationUser { get; set; }
  }
}
