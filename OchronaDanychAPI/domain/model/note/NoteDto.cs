namespace OchronaDanychAPI.domain.model.note
{
    public class NoteDto
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsEncrypted { get; set; }
        public bool IsPublic { get; set; }
        public int OwnerId { get; set; }
    }
}
