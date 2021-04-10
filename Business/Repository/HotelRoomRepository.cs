using AutoMapper;
using Business.Repository.IRepository;
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
  public class HotelRoomRepository : IHotelRoomRepository
  {
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public HotelRoomRepository(ApplicationDbContext db, IMapper mapper) // it is alrdy iniside the dependency injection container
      // it's getting mapper and db using dependency injection inside the constructor
      // this style is called dpendency injection
    {
      _mapper = mapper;
      _db = db;
    }
    public async Task<HotelRoomDTO> CreateHotelRoom(HotelRoomDTO hotelRoomDTO)
    {
      HotelRoom hotelRoom = _mapper.Map<HotelRoomDTO, HotelRoom>(hotelRoomDTO);
      hotelRoom.CreatedDate = DateTime.Now;
      hotelRoom.CreatedBy = "";

      // task 형태로 반환됨
      var addedHotelRoom = await _db.HotelRooms.AddAsync(hotelRoom);
      await _db.SaveChangesAsync();
      return _mapper.Map<HotelRoom, HotelRoomDTO>(addedHotelRoom.Entity); // task 형태에서 진짜 Object(Entity)를 가져오는 작업
    }

    public async Task<int> DeleteHotelRoom(int roomId)
    {
      var roomDetails = await _db.HotelRooms.FindAsync(roomId);
      if(roomDetails != null)
      {
        _db.HotelRooms.Remove(roomDetails);
        return await _db.SaveChangesAsync();
      }
      return 0;
    }

    public async Task<IEnumerable<HotelRoomDTO>> GetAllHotelRoom()
    {
      try
      {
        IEnumerable<HotelRoomDTO> hotelRoomDTOs=_mapper.Map<IEnumerable<HotelRoom>,IEnumerable<HotelRoomDTO> >( _db.HotelRooms);
        return hotelRoomDTOs;
      }catch(Exception ex)
      {
        return null;
      }
    }

    public async Task<HotelRoomDTO> GetHotelRoom(int roomId)
    {
      try
      {
        HotelRoomDTO hotelRoom = _mapper.Map<HotelRoom,HotelRoomDTO>( await _db.HotelRooms.FirstOrDefaultAsync(x => x.Id == roomId));
        return hotelRoom;
      }catch(Exception ex)
      {
        return null;
      }
    }

    public async Task<HotelRoomDTO> IsRoomUnique(string name)
    {
      try
      {
        HotelRoomDTO hotelRoom = _mapper.Map<HotelRoom, HotelRoomDTO>(await _db.HotelRooms.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()));
        return hotelRoom;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    public async Task<HotelRoomDTO> UpdateHotelRoom(int roomId, HotelRoomDTO hotelRoomDTO)
    {
      try
      {
        if(roomId == hotelRoomDTO.Id)
        {
          HotelRoom roomDetails = await _db.HotelRooms.FindAsync(roomId);

          //매개변수 안에 인자 하나만 넣으면 DTO에 있는 값들은 복사되고, 없는값들은 건들지도 안음.
          //destination 에 실존하는 object를 넣어주면 dto에 있는 값들은 고대로 복사되고, DTO에 없는 값들은 destination 에 넣어준 object의 값들을 쓰게됨
          HotelRoom room = _mapper.Map<HotelRoomDTO, HotelRoom>(hotelRoomDTO, roomDetails);
          room.UpdatedBy = "";
          room.UpdatedDate = DateTime.Now;
          var updatedRoom = _db.HotelRooms.Update(room);
          return _mapper.Map<HotelRoom, HotelRoomDTO>(updatedRoom.Entity);
        }
        else
        {
          return null;
        }
      }catch(Exception ex)
      {
        return null;
      }
    }
  }
}
