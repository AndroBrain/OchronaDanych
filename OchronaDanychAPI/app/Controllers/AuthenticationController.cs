using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManagmentAPI.data.db.user;
using ShopManagmentAPI.data.repository;
using ShopManagmentAPI.domain.model.authentication;
using AuthenticationService = ShopManagmentAPI.domain.service.user.AuthenticationService;
using IAuthenticationService = ShopManagmentAPI.domain.service.user.IAuthenticationService;

namespace ShopManagmentAPI.app.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService userService = new AuthenticationService(new UserRepository(new UserDb()));

    [AllowAnonymous]
    [HttpPost("Register")]
    public ActionResult RegisterUser([FromBody] RegisterDto user)
    {
        try
        {
            userService.RegisterUser(user);
            return Ok("Successfully added new user");
        }
        catch (ArgumentException e)
        {
            return BadRequest("User already exists");
        }
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        await Task.Delay(2000);
        var idUser = userService.FindUserByEmail(loginDTO.Email);
        if (idUser == null)
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User with given name and password doesn't exist");
        }

        var passwordVerificationResult = userService.VerifyPasswordHashes(idUser.User, loginDTO.Password);
        if (!passwordVerificationResult)
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User with given name and password doesn't exist");
        }

        return Ok(userService.GenerateJWT(idUser));
    }
}
