using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoUslug.Contract.Models
{
    public class Reference : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
