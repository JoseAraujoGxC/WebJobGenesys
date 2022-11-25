using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebJobGxCGenesys.Models {
    public class RootCL {
        public List<EntityCL> entities { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int total { get; set; }
        public string lastUri { get; set; }
        public string firstUri { get; set; }
        public string selfUri { get; set; }
        public int pageCount { get; set; }
    }

}
