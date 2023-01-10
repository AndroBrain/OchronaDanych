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
            var error = userService.RegisterUser(user);
            if (error == null)
            {
                return Ok("Successfully added new user");
            }
            return BadRequest(error);
        }
        catch (ArgumentException e)
        {
            return BadRequest("User with given email already exists");
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
            return StatusCode(StatusCodes.Status403Forbidden, "User with given email and password doesn't exist");
        }

        var passwordVerificationResult = userService.VerifyPasswordHashes(idUser.User, loginDTO.Password);
        if (!passwordVerificationResult)
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User with given email and password doesn't exist");
        }
        loginLimiter.ResetAttempts(loginDTO.Email);
        return Ok(userService.GenerateJWT(idUser));
    }
}
