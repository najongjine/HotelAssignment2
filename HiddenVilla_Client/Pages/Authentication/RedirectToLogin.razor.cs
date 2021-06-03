using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla_Client.Pages.Authentication
{
  public partial class RedirectToLogin
  {
    [Inject]
    private NavigationManager navigationManager { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> authenticationState { get; set; }
    bool notAuthorized { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
      /* 로그인이 아예 안되있으면 기존 url 백업해두고 로그인 페이지로 보내고,
       * 권한이 없는거면 RedirectoToLogin 페이지의 no access 컨텐츠를 보여주는 코드*/

      var authState = await authenticationState;
      //A?.B?.C <- A나 B나 C 중에 하나만 null이라도 null을 반환하는 식. 모두 null이 아니라면 A.B.C의 값을 반환함.
      if (authState?.User?.Identity is null || !authState.User.Identity.IsAuthenticated)
      {
        /* RedirectLoginPage 는 @page 설정을 안해줬음
       * url 이 바뀌지 않음
       * return url로 전환 하는건 login.razor.cs에서 구현해놓음
       */
        var returnUrl = navigationManager.ToBaseRelativePath(navigationManager.Uri);
        if (String.IsNullOrWhiteSpace(returnUrl))
        {
          navigationManager.NavigateTo("login", true);
        }
        else
        {
          navigationManager.NavigateTo($"login?returnUrl={returnUrl}", true);
        }
      }
      else
      {
        notAuthorized = true;
      }
    }

  }
}
