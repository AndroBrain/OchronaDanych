using ShopManagmentAPI.data.db.user;
using ShopManagmentAPI.data.mappers;
using ShopManagmentAPI.domain.model.user;
using ShopManagmentAPI.domain.repository;

namespace ShopManagmentAPI.data.repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserDao userDao;

        public UserRepository(IUserDao userDao)
        {
            this.userDao = userDao;
        }
        public IdUser? Create(User user)
        {
            var userEntity = userDao.Create(UserMapper.ModelToEntity(user));
            if (userEntity == null) throw new ArgumentException("User already exists");
            return UserMapper.EntityToModel(userEntity);
        }

        public IdUser? Get(int id)
        {
            var userEntity = userDao.Get(id);
            if (userEntity == null) return null;
            return UserMapper.EntityToModel(userEntity);
        }

        public IdUser? GetByEmail(string email)
        {
            var userEntity = userDao.GetByEmail(email);
            if (userEntity == null) return null;
            return UserMapper.EntityToModel(userEntity);
        }
    }
}
