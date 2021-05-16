using DataAccess.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
  public interface IRoomOrderDetailsRepository
  {
    public Task<RoomOrderDetailsDTO> Create(RoomOrderDetailsDTO);
  }
}
