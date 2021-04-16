using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAssignment2.Service.IService
{
  public interface IFileUpload
  {
    Task<string> UploadFile(IBrowserFile file);
    bool DeleteFile(string fileName);
  }
}
