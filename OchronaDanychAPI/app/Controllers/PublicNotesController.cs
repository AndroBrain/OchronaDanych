using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OchronaDanychAPI.data.db.note;
using OchronaDanychAPI.data.repository;
using OchronaDanychAPI.domain.model.note;
using OchronaDanychAPI.domain.repository;
using ShopManagmentAPI.data.db.user;
using ShopManagmentAPI.data.repository;
using ShopManagmentAPI.domain.repository;
using ShopManagmentAPI.domain.service.user;

namespace OchronaDanychAPI.app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicNoteController : ControllerBase
    {
        private readonly INoteRepository noteRepository = new NoteRepository(new NoteDb());

        [HttpGet("GetNotes")]
        public ActionResult<List<NoteDisplayDto>> GetNotes()
        {
            return Ok(noteRepository.GetPublicNotes());
        }

        [HttpGet("GetNote")]
        public ActionResult<PublicNoteDto> GetNote(int id)
        {
            var note = noteRepository?.GetNote(id);
            if (note is null)
            {
                return NotFound();
            }
            if (!note.IsPublic)
            {
                return Forbid();
            }
            return Ok(
                new PublicNoteDto()
                {
                    Name = note.Name,
                    Description = note.Description
                }
            );
        }
    }
}
