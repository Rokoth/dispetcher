using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoUslug.Contract.Models
{
    public class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ChangeDate { get; set; } = DateTimeOffset.Now;

        public Guid? CreateUser { get; set; }
        public Guid? ChangeUser { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
