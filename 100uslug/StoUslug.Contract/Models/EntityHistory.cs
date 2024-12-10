namespace StoUslug.Contract.Models
{
    public class EntityHistory
    {
        public int HId { get; set; }
        public Guid Id { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ChangeDate { get; set; }

        public Guid? CreateUser { get; set; }
        public Guid? ChangeUser { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
