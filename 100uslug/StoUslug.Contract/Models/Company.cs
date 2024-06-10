using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoUslug.Contract.Models
{
    public class Company : Entity
    {
        public string Name { get; set; }
        public string FullName { get; set; }

        public List<Document> Documents { get; set; }
        public List<Contact> Contacts { get; set; }
    }

    /// <summary>
    /// Filter class
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public abstract class Filter<T> : IFilter<T> where T : Entity
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="size">Page size</param>
        /// <param name="page">Page number</param>
        /// <param name="sort">Sort field</param>
        public Filter(int? size, int? page, string sort)
        {
            Size = size;
            Page = page;
            Sort = sort;
        }
        /// <summary>
        /// Page size
        /// </summary>
        public int? Size { get; }
        /// <summary>
        /// Page number
        /// </summary>
        public int? Page { get; }
        /// <summary>
        /// Sort field
        /// </summary>
        public string Sort { get; }
    }

    /// <summary>
    /// Обобщенный интерфейс классов фильтра
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFilter<T> where T : Entity
    {
        /// <summary>
        /// Страница
        /// </summary>
        int? Page { get; }
        /// <summary>
        /// Размер
        /// </summary>
        int? Size { get; }
        /// <summary>
        /// Поле сортировки
        /// </summary>
        string Sort { get; }
    }
}
