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
  public class HotelImagesRepository : IHotelImageRepository
  {
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public HotelImagesRepository(ApplicationDbContext db, IMapper mapper) // it is alrdy iniside the dependency injection container
                                                                          // it's getting mapper and db using dependency injection inside the constructor
                                                                          // this style is called dpendency injection
    {
      _mapper = mapper;
      _db = db;
    }


    public async Task<int> CreateHotelRoomImage(HotelRoomImageDTO imageDTO)
    {
      var image = _mapper.Map<HotelRoomImageDTO, HotelRoomImage>(imageDTO);
      await _db.HotelRoomsImages.AddAsync(image);
      return await _db.SaveChangesAsync();
    }

    public async Task<int> DeleteHotelRoomImageByRoomId(int roomId)
    {
      var imageList = await _db.HotelRoomsImages.Where(x => x.RoomId == roomId).ToListAsync();
      _db.HotelRoomsImages.RemoveRange(imageList);
      return await _db.SaveChangesAsync();
    }

    public async Task<int> DeleteHotelRoomImageById(int imageId)
    {
      var image = await _db.HotelRoomsImages.FindAsync(imageId);
      _db.HotelRoomsImages.Remove(image);
      return await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<HotelRoomImageDTO>> GetHotelRoomImages(int roomId)
    {
      return _mapper.Map<IEnumerable<HotelRoomImage>, IEnumerable<HotelRoomImageDTO>>(
      await _db.HotelRoomsImages.Where(x => x.RoomId == roomId).ToListAsync());
    }
  }
}
