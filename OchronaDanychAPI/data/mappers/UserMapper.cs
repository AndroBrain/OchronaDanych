using ShopManagmentAPI.data.entities;
using ShopManagmentAPI.domain.model.user;

namespace ShopManagmentAPI.data.mappers;

public class UserMapper
{
    public static UserEntity ModelToEntity(User user)
    {
        var entity = new UserEntity
        {
            Email = user.Email,
            Name = user.Name,
            PasswordHash = user.PasswordHash
        };
        return entity;
    }

    public static UserEntity IdModelToEntity(IdUser idUser)
    {
        var entity = new UserEntity
        {
            Id = idUser.Id,
            Email = idUser.User.Email,
            Name = idUser.User.Name,
            PasswordHash = idUser.User.PasswordHash
        };
        return entity;
    }

    public static IdUser EntityToModel(UserEntity entity)
    {
        var idUser = new IdUser
        {
            Id = entity.Id,
            User = new User()
            {
                Email = entity.Email,
                Name = entity.Name,
                PasswordHash= entity.PasswordHash,
            }
        };
        return idUser;
    }
}
