﻿using HiddenVilla_Client.Service.IService;
using Microsoft.AspNetCore.Components;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla_Client.Pages.Authentication
{
  public partial class Register
  {
    private UserRequestDTO UserForRegistration { get; set; } = new UserRequestDTO();
    public bool IsProcessing { get; set; } = false;
    public bool ShowRegistrationErrors { get; set; } = false;
    public IEnumerable<string> Errors { get; set; }

    [Inject]
    public IAuthenticationService authenticationService { get; set; }
    [Inject]
    public NavigationManager navigationManager { get; set; }

    private async Task RegisterUser()
    {
      ShowRegistrationErrors = false;
      IsProcessing = true;
      var result = await authenticationService.RegisterUser(UserForRegistration);
      if (result.IsRegistrationSuccessful)
      {
        IsProcessing = false;
        navigationManager.NavigateTo("/login");
      }
      else
      {
        IsProcessing = false;
        Errors = result.Errors;
        ShowRegistrationErrors = true;
      }
    }
  }
}
