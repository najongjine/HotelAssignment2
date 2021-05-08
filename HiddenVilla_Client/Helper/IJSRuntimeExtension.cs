using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla_Client.Helper
{
  public static class IJSRuntimeExtension
  {
    // this IJSRuntime JSRuntime = IJSRuntime 에게 ToastrSuccess 라는 함수를 장착 시키는 코드
    public static async ValueTask ToastrSuccess(this IJSRuntime JSRuntime, string message)
    {
      await JSRuntime.InvokeVoidAsync("ShowToastr", "success", message);
    }
    public static async ValueTask ToastrError(this IJSRuntime JSRuntime, string message)
    {
      await JSRuntime.InvokeVoidAsync("ShowToastr", "error", message);
    }

    public static async Task SweetAlertSuccess(this IJSRuntime JsRuntime, string message)
    {
      await JsRuntime.InvokeVoidAsync("ShowSweetAlert", "success",message);
    }
    public static async Task SweetAlertError(this IJSRuntime JsRuntime, string message)
    {
      await JsRuntime.InvokeVoidAsync("ShowSweetAlert", "error", message);
    }
  }
}
