using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebJobDinersHT1;

namespace WebJobGxCGenesys.Models {
    public class EntityCL {
        public string id { get; set; }
        public string name { get; set; }
        public DivisionCL division { get; set; }
        public List<string> columnNames { get; set; }
        public List<PhoneColumnCL> phoneColumns { get; set; }
        public string selfUri { get; set; }
    }

}
