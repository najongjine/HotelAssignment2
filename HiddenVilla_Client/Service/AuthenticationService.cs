using Blazored.LocalStorage;
using Common;
using HiddenVilla_Client.Service.IService;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HiddenVilla_Client.Service
{
  public class AuthenticationService : IAuthenticationService
  {
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorage)
    {
      _httpClient = httpClient;
      _localStorage = localStorage;
    }

    public Task Logout()
    {
      throw new NotImplementedException();
    }

    public Task<RegistrationResponseDTO> RegisterUser(UserRequestDTO userForRegistration)
    {
      throw new NotImplementedException();
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
