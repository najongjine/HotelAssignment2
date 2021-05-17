using AutoMapper;
using Business.Repository.IRepository;
using Common;
using DataAccess.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
  public class RoomOrderDetailsRepository : IRoomOrderDetailsRepository
  {
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    // it is alrdy iniside the dependency injection container
    // it's getting mapper and db using dependency injection inside the constructor
    // this style is called dpendency injection
    public RoomOrderDetailsRepository(ApplicationDbContext db, IMapper mapper)
    {
      _mapper = mapper;
      _db = db;
    }
    public async Task<RoomOrderDetailsDTO> Create(RoomOrderDetailsDTO details)
    {
      try
      {
        // datetime to date
        details.CheckInDate = details.CheckInDate.Date;
        details.CheckOutDate = details.CheckOutDate.Date;
        var roomOrder = _mapper.Map<RoomOrderDetailsDTO, RoomOrderDetails>(details);
        roomOrder.Status = SD.Status_Pending;
        var result = await _db.RoomOrderDetails.AddAsync(roomOrder);
        await _db.SaveChangesAsync();
        return _mapper.Map<RoomOrderDetails, RoomOrderDetailsDTO>(result.Entity);
      }
      catch (Exception e)
      {
        return null;
      }
    }

    public Task<IEnumerable<RoomOrderDetailsDTO>> GetAllRoomOrderDetails()
    {
      throw new NotImplementedException();
    }

    public Task<RoomOrderDetailsDTO> GetRoomOrderDetail(int roomOrderId)
    {
      throw new NotImplementedException();
    }

    public Task<bool> IsRoomBooked(int RoomId, DateTime checkInDate, DateTime checkOutDate)
    {
      throw new NotImplementedException();
    }

    public Task<RoomOrderDetailsDTO> MarkPaymentSuccessful(int id)
    {
      throw new NotImplementedException();
    }

    public Task<bool> UpdateOrderStatus(int RoomOrderId, string Status)
    {
      throw new NotImplementedException();
    }
  }
}
