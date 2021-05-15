using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
  public class HotelAmenityDTO
  {
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    public string Timming { get; set; }

    [StringLength(3000)]
    public string Description { get; set; }
    public string Icon { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
  }
}
