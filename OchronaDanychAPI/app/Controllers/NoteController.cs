using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OchronaDanychAPI.data.db.note;
using OchronaDanychAPI.data.repository;
using OchronaDanychAPI.domain.model.note;
using OchronaDanychAPI.domain.repository;
using ShopManagmentAPI.data.db.user;
using ShopManagmentAPI.data.repository;
using ShopManagmentAPI.domain.model.user;
using ShopManagmentAPI.domain.repository;
using ShopManagmentAPI.domain.service.user;

namespace ShopManagmentAPI.app.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class NoteController : ControllerBase
{
    private readonly IUserRepository userRepository = new UserRepository(new UserDb());
    private readonly IAuthenticationService authSerivce;
    private readonly INoteRepository noteRepository = new NoteRepository(new NoteDb());

    public NoteController()
    {
        authSerivce = new AuthenticationService(userRepository);
    }

    [HttpPost("AddNote")]
    public ActionResult AddNote([FromBody] CreateNoteDto createNoteDto)
    {
        var user = authSerivce.GetUserFromToken(HttpContext);
        if (user is null)
        {
            return Unauthorized();
        }
        var note = new NoteDto()
        {
            Name = createNoteDto.Name,
            Description = createNoteDto.Description,
            IsEncrypted = false,
            IsPublic = false,
            OwnerId = user.Id,
        };
        noteRepository.CreateNote(note);
        return Ok();
    }

    [HttpGet("GetNotes")]
    public ActionResult<List<NoteDisplayDto>> GetNotes()
    {
        var user = authSerivce.GetUserFromToken(HttpContext);
        if (user is null)
        {
            return Unauthorized();
        }
        return Ok(noteRepository.GetUserNotes(user.Id));
    }

    [HttpGet("GetNote")]
    public ActionResult<NoteDto> GetNote(int id)
    {
        var note = noteRepository.GetNote(id);
        if (note is null) return NotFound();
        if (note.IsPublic)
        {
            return Ok(note);
        }
        var user = authSerivce.GetUserFromToken(HttpContext);
        if (user is null)
        {
            return Unauthorized();
        }
        var notes = noteRepository.GetUserNotes(user.Id);
        if (notes.FirstOrDefault(note => note.Id == id, null) != null)
        {
            return Ok(note);
        }
        return NotFound();
    }

    [HttpPost("EncryptNote")]
    public ActionResult EncryptNote([FromBody] EncryptNoteDto encryptNoteDto)
    {
        var user = authSerivce.GetUserFromToken(HttpContext);
        if (user is null)
        {
            return Unauthorized();
        }
        int id = encryptNoteDto.Id;
        var note = noteRepository.GetNote(id);

        var notes = noteRepository.GetUserNotes(user.Id);
        if (notes.FirstOrDefault(note => note?.Id == id, null) != null)
        {
            if (note.IsPublic || note.IsEncrypted)
            {
                return BadRequest();
            }
            noteRepository.EncryptNote(note, encryptNoteDto.Password);
            return Ok();
        }
        return NotFound();
    }

    [HttpPost("DecryptNote")]
    public ActionResult<string> DecryptNote([FromBody] EncryptNoteDto encryptNoteDto) {
        var user = authSerivce.GetUserFromToken(HttpContext);
        if (user is null)
        {
            return Unauthorized();
        }
        int id = encryptNoteDto.Id;
        var note = noteRepository.GetNote(id);

        var notes = noteRepository.GetUserNotes(user.Id);
        if (notes.FirstOrDefault(note => note?.Id == id, null) != null)
        {
            if (note.IsPublic || !note.IsEncrypted)
            {
                return BadRequest();
            }
            var description = noteRepository.DecryptNote(id, encryptNoteDto.Password);
            if (description is null)
            {
                return Forbid();
            }
            return Ok(description);
        }
        return NotFound();
    }

    [HttpPut("PublishNote")]
    public ActionResult PublishNote(int id)
    {
        var user = authSerivce.GetUserFromToken(HttpContext);
        if (user is null)
        {
            return Unauthorized();
        }
        var note = noteRepository.GetNote(id);
        if (note is null)
        {
            return NotFound();
        }
        var notes = noteRepository.GetUserNotes(user.Id);
        if (notes.FirstOrDefault(note => note?.Id == id, null) == null)
        {
            return NotFound();
        }
        if (note.IsPublic || note.IsEncrypted)
        {
            return BadRequest();
        }
        noteRepository.PublishNote(note);
        return Ok();
    }
}
