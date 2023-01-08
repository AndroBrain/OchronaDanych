using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OchronaDanychAPI.domain;
using ShopManagmentAPI.data.db;
using ShopManagmentAPI.data.repository;
using ShopManagmentAPI.domain.model.authentication;
using ShopManagmentAPI.domain.model.user;
using ShopManagmentAPI.domain.repository;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ShopManagmentAPI.domain.service.user;

public class AuthenticationService : IAuthenticationService
{
    private readonly IPasswordHasher<User> passwordHasher = new PasswordHasher<User>();
    private readonly IUserRepository userRepository;
    public AuthenticationService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public string? RegisterUser(RegisterDto user)
    {
        if (!new EmailAddressAttribute().IsValid(user.Email))
        {
            return "Invalid Email";
        }
        if (!PasswordChecker.IsStrongEnough(user.Password))
        {
            return "Password must contain 1 lowercase character, 1 uppercase character, 1 number, 1 special character and be at least 8 letters long";
        }
        if (PasswordChecker.IsTooLong(user.Password))
        {
            return "Password too long";
        }
        if (!PasswordChecker.IsEntropyBigEnough(user.Password))
        {
            return "Password entropy is too low";
        }
        var newUser = new User()
        {
            Email = user.Email,
            Name = user.Name,
        };
        newUser.PasswordHash = passwordHasher.HashPassword(newUser, user.Password);
        userRepository.Create(newUser);
        return null;
    }

    public IdUser? FindUserByEmail(string email)
    {
        return userRepository.GetByEmail(email);
    }

    public bool VerifyPasswordHashes(User user, string loginPassword)
    {
        return passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginPassword) == PasswordVerificationResult.Success;
    }
    public string GenerateJWT(IdUser idUser)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, idUser.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationSettings.Key));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(AuthenticationSettings.ExpireDays);

        var token = new JwtSecurityToken(AuthenticationSettings.Issuer,
            AuthenticationSettings.Issuer,
            claims,
            expires: expires,
            signingCredentials: cred);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public IdUser? GetUserFromToken(HttpContext context)
    {
        var id = context.User.Identities.First()?.Claims?.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
            return null;
        return userRepository.Get(int.Parse(id));
    }

}
