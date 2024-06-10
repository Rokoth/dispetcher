using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoUslug.Db.Model
{
    public class Table: Entity
    {       
        public string Name { get; set; }
        public string ModelName { get; set; }
        public virtual ICollection<Column> Columns { get; set; }
        public virtual ICollection<ForeignKey> ForeignKeys { get; set; }
        public virtual ICollection<TableIndex> Indexes { get; set; }
    }

    public class Column : Entity
    {        
        public Guid TableId { get; set; }
        public string Name { get; set; }
        public string ModelName { get; set; }
        public string Type { get; set; }
        public bool IsNullable { get; set; }
        public bool AutoIncrement { get; set; }
        public bool IsPKey { get; set; }
        public string DefaultValue { get; set; }
    }

    public class ForeignKey : Entity
    {       
        public Guid TableId { get; set; }
        public string Name { get; set; }
        public string ForeignTable { get; set; }
        public string ForeignField { get; set; }
        public string KeyField { get; set; }
    }

    public class TableIndex : Entity
    {       
        public Guid TableId { get; set; }
        public string Name { get; set; }
        public string Field { get; set; }
        public bool IsUnique { get; set; }
    }
}
