using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManagmentAPI.data.db.user;
using ShopManagmentAPI.data.repository;
using ShopManagmentAPI.domain.model.user;
using ShopManagmentAPI.domain.repository;
using ShopManagmentAPI.domain.service.user;

namespace ShopManagmentAPI.app.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserRepository userRepository = new UserRepository(new UserDb());
    private readonly IAuthenticationService authSerivce;

    public UserController()
    {
        authSerivce = new AuthenticationService(userRepository);
    }

    [HttpGet("GetUserInfo")]
    public ActionResult<UserInfoDto> GetUserInfo()
    {
        var user = authSerivce.GetUserFromToken(HttpContext)?.User;
        if (user is null)
        {
            return Unauthorized();
        }
        var result = new UserInfoDto()
        {
            Name = user.Name,
            Email = user.Email
        };
        return Ok(result);
    }
}
