using ShopManagmentAPI.data.entities;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace OchronaDanychAPI.data.entities
{
    public class NoteEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public byte[]? Encrypted { get; set; } = null;
        public byte[]? Key { get; set; } = null;
        public bool IsPublic { get; set; }
        [ForeignKey(typeof(UserEntity))]
        public int OwnerId { get; set; }
        [ManyToOne]
        public UserEntity Owner { get; set; }
    }
}
