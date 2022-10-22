using System;

namespace StoUslug.Db.Attributes
{
    public class ColumnTypeAttribute : Attribute
    {
        public string Name { get; }

        public ColumnTypeAttribute(string name)
        {
            Name = name;
        }
    }
}
