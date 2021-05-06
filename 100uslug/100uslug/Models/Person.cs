using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _100uslug.Models
{
    public class Person : Entity
    {
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public List<Document> Documents { get; set; }
        public List<Contact> Contacts { get; set; }

        public string FullName { get { return GetNamePart(Name) + GetNamePart(MiddleName) + GetNamePart(LastName); } }

        private string GetNamePart(string name)
        {
            return (string.IsNullOrEmpty(name) ? "" : name + " ");
        }
    }
}
