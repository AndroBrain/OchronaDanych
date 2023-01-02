using Microsoft.IdentityModel.Tokens;
using ShopManagmentAPI.domain.model.authentication;
using ShopManagmentAPI.domain.model.user;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace ShopManagmentAPI.domain.service.user;

public interface IAuthenticationService
{
    public void RegisterUser(RegisterDto user);
    public IdUser? FindUserByEmail(string email);
    public bool VerifyPasswordHashes(User user, string loginPassword);
    public string GenerateJWT(IdUser idUser);
    public IdUser? GetUserFromToken(HttpContext context);
}
