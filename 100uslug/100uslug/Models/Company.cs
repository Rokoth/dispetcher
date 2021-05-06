using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _100uslug.Models
{
    public class Company : Entity
    {
        public string Name { get; set; }
        public string FullName { get; set; }

        public List<Document> Documents { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
