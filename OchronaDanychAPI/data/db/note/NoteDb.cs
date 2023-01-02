using OchronaDanychAPI.data.entities;
using ShopManagmentAPI.domain.model.user;
using ShopManagmentAPI.domain;
using SQLite;
using SQLiteNetExtensions.Extensions;
using ShopManagmentAPI.data.entities;

namespace OchronaDanychAPI.data.db.note
{
    public class NoteDb : INoteDao
    {
        public void CreateNote(NoteEntity note)
        {
            using (SQLiteConnection conn = new SQLiteConnection(DbSettings.dbPath))
            {
                var owner = conn.GetWithChildren<UserEntity>(note.OwnerId);
                conn.Insert(note);
                owner.notes.Add(note);
                conn.UpdateWithChildren(owner);
                conn.UpdateWithChildren(note);
            }
        }

        public NoteEntity? GetNote(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(DbSettings.dbPath))
            {
                try
                {
                    return conn.GetWithChildren<NoteEntity?>(id);
                }
                catch (InvalidOperationException e)
                {
                    return null;
                }
            }
        }

        public List<NoteEntity> GetPublicNotes()
        {
            using (SQLiteConnection conn = new SQLiteConnection(DbSettings.dbPath))
            {
                return conn.GetAllWithChildren<NoteEntity>().Where(note => note.IsPublic).ToList();
            }
        }

        public List<NoteEntity> GetUserNotes(int ownerId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(DbSettings.dbPath))
            {
                return conn.GetAllWithChildren<NoteEntity>().Where(s => s.OwnerId == ownerId).ToList();
            }
        }

        public bool Update(NoteEntity note)
        {
            using (SQLiteConnection conn = new SQLiteConnection(DbSettings.dbPath))
            {
                var result = conn.Update(note) > 0;
                return result;
            }
        }
    }

}
