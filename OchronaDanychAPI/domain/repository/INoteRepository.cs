using OchronaDanychAPI.domain.model.note;

namespace OchronaDanychAPI.domain.repository
{
    public interface INoteRepository
    {
        public void CreateNote(NoteDto noteDto);
        public List<NoteDisplayDto> GetPublicNotes();
        public List<NoteDisplayDto> GetUserNotes(int userId);
        public NoteDto? GetNote(int id);
        public void EncryptNote(EncryptNoteDto encryptNoteDto);
        public bool PublishNote(NoteDto note);
    }
}
