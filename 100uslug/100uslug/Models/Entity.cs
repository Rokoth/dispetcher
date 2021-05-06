using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _100uslug.Models
{
    public class Entity
    {
        public Guid Id { get; set; }

        public DateTimeOffset BeginDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
