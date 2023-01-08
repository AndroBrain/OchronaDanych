using ShopManagmentAPI.domain.model.user;
using System.ComponentModel.DataAnnotations;

namespace ShopManagmentAPI.domain.model.authentication
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        [MinLength(8)]
        [MaxLength(32)]
        public string Password { get; set; }
    }
}
