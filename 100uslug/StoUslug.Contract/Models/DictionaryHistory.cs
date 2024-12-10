namespace StoUslug.Contract.Models
{
    public class DictionaryHistory : EntityHistory
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string SysName { get; set; } = "";
        public string DbName { get; set; } = "";

        public bool IsPeriodical { get; set; } = false;
    }
}
