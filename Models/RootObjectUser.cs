using System.Collections.Generic;

namespace WebJobGxCGenesys.Models {
    public class RootObjectUser {
        public List<Entity> entities { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int total { get; set; }
        public string firstUri { get; set; }
        public string selfUri { get; set; }
        public string lastUri { get; set; }
        public int pageCount { get; set; }
    }

}
