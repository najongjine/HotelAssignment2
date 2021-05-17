using AutoMapper;
using Business.Repository.IRepository;
using Common;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<RoomOrderDetailsDTO>> GetAllRoomOrderDetails()
    {
      try
      {
        var roomOrders = _mapper.Map<IEnumerable<RoomOrderDetails>, IEnumerable<RoomOrderDetailsDTO>>
          (_db.RoomOrderDetails.Include(u => u.HotelRoom));
        return roomOrders;
      }
      catch (Exception e)
      {
        return null;
      }
    }

    public async Task<RoomOrderDetailsDTO> GetRoomOrderDetail(int roomOrderId)
    {
      try
      {
        var roomOrder = await _db.RoomOrderDetails.Include(u => u.HotelRoom).ThenInclude(x => x.HotelRoomImages)
          .FirstOrDefaultAsync(u => u.Id == roomOrderId);
        var roomOrderDetailsDTO = _mapper.Map<RoomOrderDetails, RoomOrderDetailsDTO>(roomOrder);
        roomOrderDetailsDTO.HotelRoomDTO.TotalDays = roomOrderDetailsDTO.CheckOutDate.Subtract(roomOrderDetailsDTO.CheckInDate).Days;
        return roomOrderDetailsDTO;
      }
      catch (Exception e)
      {
        return null;
      }
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
