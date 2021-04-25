using DataAccess.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
  public interface IHotelAmenityRepository
  {
    public Task<IEnumerable<HotelAmenityDTO>> GetAllAmenities();

    public Task<HotelAmenityDTO> GetAmenity(int Id);
    public Task<HotelAmenityDTO> UpdateAmenity(int Id, HotelAmenityDTO hotelAmenityDTO);
    public Task<int> DeleteHotelAmenity(int Id);
    public Task<HotelAmenityDTO> IsNameUnique(string amenityName, int amenityId=0);

    public Task<HotelAmenityDTO> InsertAmenity(HotelAmenityDTO amenityDTO);
  }
}
