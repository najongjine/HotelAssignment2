using Blazored.LocalStorage;
using Common;
using HiddenVilla_Client.Helper;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HiddenVilla_Client.Service
{
  public class AuthStateProvider : AuthenticationStateProvider
  {
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
    {
      _httpClient = httpClient;
      _localStorage = localStorage;
    }
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
      var token = await _localStorage.GetItemAsync<string>(SD.Local_Token);
      if (token == null)
      {
        //pass anonymous identity
        //return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        //pass custom identity
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(
          new[] { new Claim(ClaimTypes.Name, "najongjin3@hotmail.com"), new Claim(ClaimTypes.Role, "master") }, "jwtAuthType"
          )));
      }
      // putting token in header auto mode.
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
      //pass roles
      return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token),"jwtAuthType")));

    }
  }
}
