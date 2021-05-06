using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _100uslug.Models
{
    public class Document : Entity
    {
        public DocumentType Type { get; set; }
        public string Seria { get; set; }
        public string Number { get; set; }
        public DateTime GetDate { get; set; }
        public bool IsMain { get; set; }
    }
}
