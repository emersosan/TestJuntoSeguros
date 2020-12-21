using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetDevPack.Identity.Jwt;
using NetDevPack.Identity.Jwt.Model;
using NetDevPack.Identity.Model;
using TestJuntoSeguros.Domain.Model.User;

namespace TestJuntoSeguros.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccountController : ControllerBase
  {
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppJwtSettings _appJwtSettings;

    public AccountController(
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IOptions<AppJwtSettings> appJwtSettings)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _appJwtSettings = appJwtSettings.Value;
    }

    [HttpGet]
    [Route("getByName")]
    [AllowAnonymous]
    public async Task<ActionResult> FindByName(string name)
    {
      var user = await _userManager.FindByNameAsync(name);
      if(user != null)
        return Ok(new UserView { Email = user.Email, Name = user.UserName, Phone = user.PhoneNumber });

      ModelState.AddModelError("", "Este usuário não existe.");
      return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
    }

    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register(RegisterUser registerUser)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

      var user = new IdentityUser
      {
        UserName = registerUser.Email,
        Email = registerUser.Email,
        EmailConfirmed = true
      };

      var result = await _userManager.CreateAsync(user, registerUser.Password);

      if (result.Succeeded)
      {
        return Ok(GetFullJwt(user.Email));
      }
      else
      {
        return BadRequest(result.Errors.Select(e => e.Description).ToArray());
      }
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginUser loginUser)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

      var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

      if (result.Succeeded)
      {
        return Ok(GetFullJwt(loginUser.Email));
      }

      if (result.IsLockedOut)
      {
        ModelState.AddModelError("", "Este usuário está temporariamente bloqueado.");
        return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
      }

      ModelState.AddModelError("", "Usuário ou senha incorretos");
      return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
    }

    [HttpDelete]
    [Authorize]
    [Route("delete")]
    public async Task<IActionResult> Delete(string name)
    {
      var user = await _userManager.FindByNameAsync(name);
      if (user != null)
      {
        await _userManager.DeleteAsync(user);
        return Ok();
      }

      ModelState.AddModelError("", "Este usuário não existe.");
      return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
    }

    [HttpGet]
    [Route("changePassword")]
    public async Task<ActionResult> ChangePassword(UserChangePassword changePassword)
    {
      var user = await _userManager.FindByNameAsync(User.Identity.Name);
      if (user != null)
      {
        var result = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
        if (result.Succeeded)
          return Ok();

        ModelState.AddModelError("", "Usuário ou senha incorretos");
        return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
      }
      else
      {
        ModelState.AddModelError("", "Este usuário não existe.");
        return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
      }
    }

    private string GetFullJwt(string email)
    {
      return new JwtBuilder()
          .WithUserManager(_userManager)
          .WithJwtSettings(_appJwtSettings)
          .WithEmail(email)
          .WithJwtClaims()
          .WithUserClaims()
          .WithUserRoles()
          .BuildToken();
    }
  }
}
