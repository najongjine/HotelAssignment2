using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
  public class HotelAmenity
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Timming { get; set; }
    public string Icon { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string UpdatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
  }
}
