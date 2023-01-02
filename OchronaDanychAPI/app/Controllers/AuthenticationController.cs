using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OchronaDanychAPI.app.limit;
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
    private readonly LoginLimiter loginLimiter = new LoginLimiter();

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
        if (!loginLimiter.CanLogin(loginDTO.Email))
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, "Too Many Request try again in 10 minutes");
        }
        loginLimiter.AddAttempt(loginDTO.Email);
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
        loginLimiter.ResetAttempts(loginDTO.Email);
        return Ok(userService.GenerateJWT(idUser));
    }
}
