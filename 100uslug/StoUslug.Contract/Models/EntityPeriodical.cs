namespace StoUslug.Contract.Models
{
    public class EntityPeriodical: Entity
    {
        public DateTimeOffset BeginDate { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset EndDate { get; set; } = DateTimeOffset.Now;
    }
}
