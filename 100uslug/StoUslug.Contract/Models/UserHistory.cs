namespace StoUslug.Contract.Models
{
    public class UserHistory : EntityHistory
    {
        public string Name { get; set; } = "";
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
