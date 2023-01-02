using ShopManagmentAPI.data.entities;

namespace ShopManagmentAPI.domain.model.user
{
    public class User
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}
