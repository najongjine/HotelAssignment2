using HotelAssignment2.Service.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAssignment2.Service
{
  
  public class FileUpload : IFileUpload
  {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;
    public FileUpload(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
      Console.WriteLine("## _Env: " + HotelAssignment2.Startup._Env);
      _webHostEnvironment = webHostEnvironment;
      _configuration = configuration;
    }
    public bool DeleteFile(string fileName)
    {
      try
      {
        var path = $"{_webHostEnvironment.WebRootPath}/RoomImages/{fileName}";
        if (File.Exists(path))
        {
          File.Delete(path);
          return true;
        }
        return false;
      }
      catch(Exception ex)
      {
        throw ex;
      }
    }

    public async Task<string> UploadFile(IBrowserFile file)
    {
      int MAXALLOWEDSIZE=500*1024*1024;
      try
      {
        FileInfo fileInfo=new FileInfo(file.Name);
        var fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
        var folderDirectory = $"{_webHostEnvironment.WebRootPath}/RoomImages";
        var path = Path.Combine(_webHostEnvironment.WebRootPath,"RoomImages",fileName);
        Console.WriteLine("## folderDirectory: " + folderDirectory);
        Console.WriteLine("## path: " + path);

        var memoryStream = new MemoryStream();

        /* file.OpenReadStream() 안에다가 byte를 넣어주면 서버에서 받는 최고한도의 파일용량이 된다 */
        await file.OpenReadStream(MAXALLOWEDSIZE).CopyToAsync(memoryStream);
        if (!Directory.Exists(folderDirectory))
        {
          Directory.CreateDirectory(folderDirectory);
        }
        await using(var fs=new FileStream(path, FileMode.Create, FileAccess.Write))
        {
          memoryStream.WriteTo(fs);
        }
        var url = $"{_configuration.GetValue<string>("ServerURL")}";
        if (HotelAssignment2.Startup._Env.ToLower()== "Development".ToLower())
        {
          url = $"{_configuration.GetValue<string>("LocalServerURL")}";
          
        }
        
        Console.WriteLine("## url: " + url);
        var fullPath = $"{url}RoomImages/{fileName}";
        return fullPath;
      }
      catch(Exception ex)
      {
        throw ex;
      }
    }
  }
}
