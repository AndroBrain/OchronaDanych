using OchronaDanychAPI.data.entities;

namespace OchronaDanychAPI.data.db.note
{
    public interface INoteDao
    {
        public void CreateNote(NoteEntity note);
        public NoteEntity? GetNote(int id);
        public List<NoteEntity> GetPublicNotes();
        public List<NoteEntity> GetUserNotes(int ownerId);
        public bool Update(NoteEntity note);
    }
}
