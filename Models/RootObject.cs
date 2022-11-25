using System.Collections.Generic;

namespace WebJobGxCGenesys.Models{
    public class RootObject {
        public List<Conversation> conversations { get; set; }
        public string cursor { get; set; }
        public string totalHits { get; set; }
    }

}
