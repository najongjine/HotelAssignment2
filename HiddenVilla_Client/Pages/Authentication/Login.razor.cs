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
      var result = await authenticationService.SignIn(UserForAuthentication);
      if (result.IsAuthSuccessful)
      {
        IsProcessing = false;
        var absouleUri = new Uri(navigationManager.Uri);
        var queryParam = HttpUtility.ParseQueryString(absouleUri.Query);
        ReturnUrl = queryParam["returnUrl"];
        if (string.IsNullOrWhiteSpace(ReturnUrl))
        {
          navigationManager.NavigateTo("/");
        }
        else
        {
          navigationManager.NavigateTo("/"+ReturnUrl);
        }
        navigationManager.NavigateTo("/login");
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
