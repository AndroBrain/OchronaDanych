namespace OchronaDanychAPI.app.limit
{
    public class LoginLimit
    {
        public int Attempts { get; set; }
        public DateTime? LastDateTime { get; set; }
    }
}
