using OchronaDanychAPI.data.entities;
using ShopManagmentAPI.domain.model.user;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ShopManagmentAPI.data.entities
{
    [Table("Users")]
    public class UserEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string Email { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        [OneToMany]
        public List<NoteEntity> notes { get; set; } = new List<NoteEntity>();
    }
}
