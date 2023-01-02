using ShopManagmentAPI.domain.model.user;

namespace ShopManagmentAPI.domain.repository
{
    public interface IUserRepository
    {
        public IdUser? Create(User user);
        public IdUser? Get(int id);
        public IdUser? GetByEmail(string email);
    }
}
