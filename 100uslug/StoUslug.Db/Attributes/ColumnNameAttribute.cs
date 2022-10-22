using System;

namespace StoUslug.Db.Attributes
{
    /// <summary>
    /// Атрибут Наименование колонки
    /// </summary>
    public class ColumnNameAttribute : Attribute
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name"></param>
        public ColumnNameAttribute(string name)
        {
            Name = name;
        }
    }
}
