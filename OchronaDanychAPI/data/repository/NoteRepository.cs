using OchronaDanychAPI.data.db.note;
using OchronaDanychAPI.data.mappers;
using OchronaDanychAPI.domain.model.note;
using OchronaDanychAPI.domain.repository;

namespace OchronaDanychAPI.data.repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly INoteDao _noteDao;
        public NoteRepository(INoteDao noteDao)
        {
            _noteDao = noteDao;
        }

        public void CreateNote(NoteDto noteDto)
        {
            _noteDao.CreateNote(NoteMapper.DtoToEntity(noteDto));
        }

        public void EncryptNote(EncryptNoteDto encryptNoteDto)
        {
            throw new NotImplementedException();
        }

        public NoteDto? GetNote(int id)
        {
            var note = _noteDao.GetNote(id);
            if (note is null) return null;
            return NoteMapper.EntityToDto(note);
        }

        public List<NoteDisplayDto> GetPublicNotes()
        {
            return _noteDao.GetPublicNotes().Select(note => new NoteDisplayDto() { Id = note.Id, Name = note.Name }).ToList();
        }

        public List<NoteDisplayDto> GetUserNotes(int userId)
        {
            return _noteDao.GetUserNotes(userId).Select(note => new NoteDisplayDto() { Id = note.Id, Name = note.Name }).ToList();
        }

        public bool PublishNote(NoteDto note)
        {
            note.IsPublic = true;
            return _noteDao.Update(NoteMapper.DtoToEntity(note));
        }
    }
}
