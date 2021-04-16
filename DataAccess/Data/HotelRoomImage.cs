using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
  public class HotelRoomImage
  {
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string RoomImageUrl { get; set; }
    
    /* telling which property is the FK */
    [ForeignKey("RoomId")]
    /* this FK belongs to which table? HotelRoom
     * virtual 키워드르 넣으면 DB에는 반영 안함
     * */
    public virtual HotelRoom HotelRoom { get; set; }
  }
}
