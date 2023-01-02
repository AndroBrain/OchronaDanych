using ShopManagmentAPI.data.entities;
using ShopManagmentAPI.domain.model.user;

namespace ShopManagmentAPI.data.db.user;

public interface IUserDao
{
    public UserEntity? Create(UserEntity user);
    public UserEntity? Get(int id);
    public UserEntity? GetByEmail(string email);
}
