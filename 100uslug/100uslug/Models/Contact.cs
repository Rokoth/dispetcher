using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _100uslug.Models
{
    public class Contact : Entity
    {
        public ContactType Type { get; set; }
        public string Value { get; set; }
    }
}
