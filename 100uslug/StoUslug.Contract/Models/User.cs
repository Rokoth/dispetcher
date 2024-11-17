using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoUslug.Contract.Models
{
    public class User: Entity
    {
        public string Name { get; set; } = "";
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";        
    }
}
