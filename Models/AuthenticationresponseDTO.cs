using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
  public class AuthenticationresponseDTO
  {
    public bool IsAuthSuccessful { get; set; }
    public string ErrorMsg { get; set; }
    public string Token { get; set; }
    public UserDTO userDTO { get; set; }
  }
}
