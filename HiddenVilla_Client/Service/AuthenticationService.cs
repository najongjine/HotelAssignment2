using Blazored.LocalStorage;
using Common;
using HiddenVilla_Client.Service.IService;
using Microsoft.AspNetCore.Components.Authorization;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HiddenVilla_Client.Service
{
  public class AuthenticationService : IAuthenticationService
  {
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
    {
      _httpClient = httpClient;
      _localStorage = localStorage;
      _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task Logout()
    {
      await _localStorage.RemoveItemAsync(SD.Local_Token);
      await _localStorage.RemoveItemAsync(SD.Local_UserDetails);
      ((AuthStateProvider)_authenticationStateProvider).NotifyUserLogout();
      _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<RegistrationResponseDTO> RegisterUser(UserRequestDTO userForRegistration)
    {
      try
      {
        var content = JsonConvert.SerializeObject(userForRegistration);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/account/signup", bodyContent);
        var contentTemp = await response.Content.ReadAsStringAsync();
        // Enitity to DTO convert is alrdy done in repository
        var result = JsonConvert.DeserializeObject<RegistrationResponseDTO>(contentTemp);
        if (response.IsSuccessStatusCode)
        {
          return new RegistrationResponseDTO { IsRegistrationSuccessful = true };
        }
        else
        {
          return result;
        }
      }
      catch (Exception e)
      {
        throw new Exception(e.Message);
      }
    }

    public async Task<AuthenticationresponseDTO> SignIn(AuthenticationDTO userFromAuthentication)
    {
      try {
        var content = JsonConvert.SerializeObject(userFromAuthentication);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/account/signin", bodyContent);
        var contentTemp = await response.Content.ReadAsStringAsync();
        // Enitity to DTO convert is alrdy done in repository
        var result = JsonConvert.DeserializeObject<AuthenticationresponseDTO>(contentTemp);
        if (response.IsSuccessStatusCode)
        {
          await _localStorage.SetItemAsync(SD.Local_Token, result.Token);
          await _localStorage.SetItemAsync(SD.Local_UserDetails, result.userDTO);
          ((AuthStateProvider)_authenticationStateProvider).NotifyUserLoggedIn(result.Token);
          _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer",result.Token);
          return new AuthenticationresponseDTO { IsAuthSuccessful = true };
        }
        else
        {
          return result;
        }
      }
      catch(Exception e)
      {
        throw new Exception(e.Message);
      }
    }

  }
}
