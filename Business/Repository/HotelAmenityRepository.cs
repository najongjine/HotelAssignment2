using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
  public class HotelAmenityRepository : IHotelAmenityRepository
  {
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    // it is alrdy iniside the dependency injection container
    // it's getting mapper and db using dependency injection inside the constructor
    // this style is called dpendency injection
    public HotelAmenityRepository(ApplicationDbContext db, IMapper mapper, ILogger<HotelAmenityRepository> logger)
    {
      _mapper = mapper;
      _db = db;
      _logger = logger;
    }
    public async Task<int> DeleteHotelAmenity(int Id)
    {
      var amenity = await _db.HotelAmenities.FindAsync(Id);
      if (amenity != null)
      {
        _db.Remove(amenity);
        return await _db.SaveChangesAsync();
      }
      return -1;
    }

    public async Task<IEnumerable<HotelAmenityDTO>> GetAllAmenities()
    {
      try
      {
        var amenitiesDTO = _mapper.Map<IEnumerable<HotelAmenity>, IEnumerable<HotelAmenityDTO>>(_db.HotelAmenities);
        return amenitiesDTO;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    public async Task<HotelAmenityDTO> GetAmenity(int Id)
    {
      try
      {
        var amenity = _mapper.Map<HotelAmenity, HotelAmenityDTO>(await _db.HotelAmenities.FindAsync(Id));
        return amenity;
      }
      catch (Exception ex)
      {
        return null;
      }

    }

    public async Task<HotelAmenityDTO> InsertAmenity(HotelAmenityDTO amenityDTO)
    {
      try
      {
        var amenity = _mapper.Map<HotelAmenityDTO, HotelAmenity>(amenityDTO);
        amenity.CreatedBy = "";
        var addedAmenity = await _db.HotelAmenities.AddAsync(amenity);
        await _db.SaveChangesAsync();
        return _mapper.Map<HotelAmenity, HotelAmenityDTO>(addedAmenity.Entity);
      }catch(Exception ex)
      {
        return null;
      }
    }

    public async Task<HotelAmenityDTO> IsNameUnique(string amenityName, int amenityId = 0)
    {
      try
      {
        if (amenityId < 1)
        {
          var amenity = _mapper.Map<HotelAmenity, HotelAmenityDTO>(await _db.HotelAmenities.FirstOrDefaultAsync(x => x.Name1.ToLower() == amenityName.ToLower()));
          return amenity;
        }
        else
        {
          var amenity = _mapper.Map<HotelAmenity, HotelAmenityDTO>(await _db.HotelAmenities.FirstOrDefaultAsync(x => x.Name1.ToLower() == amenityName.ToLower() && x.Id != amenityId));
          return amenity;
        }
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    public async Task<HotelAmenityDTO> UpdateAmenity(int Id, HotelAmenityDTO hotelAmenityDTO)
    {
      try
      {
        if (Id == hotelAmenityDTO.Id)
        {
          var exAmenity = await _db.HotelAmenities.FindAsync(Id);
          exAmenity = _mapper.Map<HotelAmenityDTO, HotelAmenity>(hotelAmenityDTO, exAmenity);
          exAmenity.CreatedBy = "";
          exAmenity.UpdatedDate = DateTime.UtcNow;
          await _db.SaveChangesAsync();
          return _mapper.Map<HotelAmenity, HotelAmenityDTO>(exAmenity);
        }
        else
        {
          return null;
        }
        
      }
      catch(Exception ex)
      {
        return null;
      }
      
    }
  }
}
