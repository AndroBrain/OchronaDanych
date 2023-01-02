using Microsoft.OpenApi.Writers;
using OchronaDanychAPI.data.entities;
using OchronaDanychAPI.domain.model.note;

namespace OchronaDanychAPI.data.mappers
{
    public class NoteMapper
    {
        public static NoteDto EntityToDto(NoteEntity entity)
        {
            return new NoteDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsPublic = entity.IsPublic,
                OwnerId = entity.OwnerId,
            };
        }

        public static NoteEntity DtoToEntity(NoteDto note)
        {
            return new NoteEntity()
            {
                Id = note.Id,
                Name = note.Name,
                Description = note.Description,
                OwnerId = note.OwnerId,
                IsPublic = note.IsPublic,
            };
        }
    }
}
