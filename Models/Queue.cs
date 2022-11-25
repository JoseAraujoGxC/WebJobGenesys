using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebJobGxCGenesys.Models {
    public class Queue {
    
        public string Id { get; set; }
        public string Name { get; set; }
        public DivisionCL division { get; set; }
    }
}