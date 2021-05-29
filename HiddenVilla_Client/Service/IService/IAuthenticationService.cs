using Microsoft.AspNetCore.Components.Authorization;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla_Client.Service.IService
{
  public interface IAuthenticationService
  {
    Task<RegistrationResponseDTO> RegisterUser(UserRequestDTO userForRegistration);

    Task<AuthenticationresponseDTO>  SignIn(AuthenticationDTO userFromAuthentication);

    Task Logout();
  }
}
