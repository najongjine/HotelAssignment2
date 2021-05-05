using Common;
using DataAccess.Data;
using HotelAssignment2_API.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelAssignment2_API.Controllers
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  [Authorize]
  public class AccountController : Controller
  {
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly APISettings _apiSettings;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager
      , IOptions<APISettings> options)
    {
      _signInManager = signInManager;
      _userManager = userManager;
      _roleManager = roleManager;

      /* startup.cs 에서 등록한 services.Configure<APISettings>(appSettingSection);   APISettings 에 있는 값을
       * _apiSettings 에 주입시키는 코드*/
      _apiSettings = options.Value;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] UserRequestDTO userRequestDTO)
    {
      if (userRequestDTO == null || !ModelState.IsValid)
      {
        return BadRequest();
      }

      var user = new ApplicationUser
      {
        UserName = userRequestDTO.Email,
        Email = userRequestDTO.Email,
        Name = userRequestDTO.Name,
        PhoneNumber = userRequestDTO.PhoneNo,
        EmailConfirmed = true
      };

      var result = await _userManager.CreateAsync(user, userRequestDTO.Password);

      if (!result.Succeeded)
      {
        var errors = result.Errors.Select(e => e.Description);
        return BadRequest(new RegistrationResponseDTO { Errors = errors, IsRegistrationSuccessful = false });
      }
      var roleResult = await _userManager.AddToRoleAsync(user, SD.Role_Customer);

      if (!roleResult.Succeeded)
      {
        var errors = result.Errors.Select(e => e.Description);
        return BadRequest(new RegistrationResponseDTO { Errors = errors, IsRegistrationSuccessful = false });
      }
      return StatusCode(201);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] AuthenticationDTO authenticationDTO)
    {
      var result = await _signInManager.PasswordSignInAsync(authenticationDTO.UserName, authenticationDTO.Password, false, false);
      if (result.Succeeded)
      {
        var user = await _userManager.FindByNameAsync(authenticationDTO.UserName);
        if (user == null)
        {
          return Unauthorized(new AuthenticationresponseDTO { IsAuthSuccessful = false, ErrorMsg = "invalid Authentication" });
        }

        //everything is valid. need to login the user.
      }
    }

    private SigningCredentials GetSigninCredentials()
    {
      var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSettings.SecretKey));
      return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims(ApplicationUser user)
    {
      var claims = new List<Claim>
      {
        // 값 설정 안해줘도 default로 셋팅됨
        new Claim(ClaimTypes.Name,user.Email)
        ,new Claim(ClaimTypes.Email,user.Email)

        // custom claim
        ,new Claim("Id",user.Id)
      };

      //role 추가해주는건 아주 중요
      var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(user.Email));

      foreach (var role in roles)
      {
        claims.Add(new Claim(ClaimTypes.Role, role));
      }
      return claims;
    }

  }
}
