using System;

namespace WebJobGxCGenesys.Models {

    public class Segment {
        public DateTime segmentStart { get; set; }
        public DateTime segmentEnd { get; set; }
        public string queueId { get; set; }
        public string disconnectType { get; set; }
        public string segmentType { get; set; }
        public bool conference { get; set; }
        public string wrapUpCode { get; set; }
    }

}
