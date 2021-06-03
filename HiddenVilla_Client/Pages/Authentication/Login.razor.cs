using HiddenVilla_Client.Service.IService;
using Microsoft.AspNetCore.Components;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HiddenVilla_Client.Pages.Authentication
{
  public partial class Login
  {
    private AuthenticationDTO UserForAuthentication { get; set; } = new AuthenticationDTO();
    public bool IsProcessing { get; set; } = false;
    public bool ShowAuthenticationErrors { get; set; } = false;
    public string Errors { get; set; }
    public string ReturnUrl { get; set; }

    [Inject]
    public IAuthenticationService authenticationService { get; set; }
    [Inject]
    public NavigationManager navigationManager { get; set; }

    private async Task LoginUser()
    {
      ShowAuthenticationErrors = false;
      IsProcessing = true;

      //service 단계에서 이미 localstorage에 토큰과 userdetails를 저장함
      var result = await authenticationService.SignIn(UserForAuthentication);
      if (result.IsAuthSuccessful)
      {
        IsProcessing = false;

        /* query parameter 에 returnUrl={watever} 이렇게 설정해준게 있으면 
         * {watever} 여기로 리다이렉트 시켜라
         */
        var absouleUri = new Uri(navigationManager.Uri);
        var queryParam = HttpUtility.ParseQueryString(absouleUri.Query);
        ReturnUrl = queryParam["returnUrl"];
        if(string.IsNullOrWhiteSpace(ReturnUrl))
        {
          navigationManager.NavigateTo("/");
        }
        else
        {
          navigationManager.NavigateTo("/"+ReturnUrl);
        }
      }
      else
      {
        IsProcessing = false;
        Errors = result.ErrorMsg;
        ShowAuthenticationErrors = true;
      }
    }

  }
}
