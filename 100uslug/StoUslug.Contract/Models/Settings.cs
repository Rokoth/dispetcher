namespace StoUslug.Contract.Models
{
    public class Settings
    {
        public Guid UserId { get; set; }
        public string Server { get; set; } = "";
        public string Name { get; set; } = "";
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
