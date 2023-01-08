using OchronaDanychAPI.data.db.note;
using OchronaDanychAPI.data.mappers;
using OchronaDanychAPI.domain.model.note;
using OchronaDanychAPI.domain.repository;
using System.Security.Cryptography;
using System.Text;

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

        public string? DecryptNote(int id, string password)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] aesKey = SHA256.Create().ComputeHash(passwordBytes);
                byte[] aesIV = MD5.Create().ComputeHash(passwordBytes);
                aes.Key = aesKey;
                aes.IV = aesIV;
                var note = _noteDao.GetNote(id);
                if (note is null || note.Encrypted == null || note.Key == null)
                {
                    return null;
                }
                if (!aesKey.SequenceEqual(note.Key))
                {
                    return null;
                }
                return AesEncryptor.DecryptStringFromBytes(note.Encrypted, aes.Key, aes.IV);
            }
        }

        public void EncryptNote(NoteDto note, string password)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] aesKey = SHA256.Create().ComputeHash(passwordBytes);
                byte[] aesIV = MD5.Create().ComputeHash(passwordBytes);
                aes.Key = aesKey;
                aes.IV = aesIV;
                byte[] encrypted = AesEncryptor.EncryptStringToBytes(note.Description, aes.Key, aes.IV);
                var entity = NoteMapper.DtoToEntity(note);
                entity.Description = null;
                entity.Encrypted = encrypted;
                entity.Key = aes.Key;
                _noteDao.Update(entity);
            }
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
