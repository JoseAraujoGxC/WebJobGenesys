using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebJobGxCGenesys.Models {
    public class Session {
        public string mediaType { get; set; }
        public string sessionId { get; set; }
        public string ani { get; set; }
        public string direction { get; set; }
        public string dnis { get; set; }
        public string outboundCampaignId { get; set; }
        public string outboundContactId { get; set; }
        public string outboundContactListId { get; set; }
        public string edgeId { get; set; }
        public string remoteNameDisplayable { get; set; }
        public List<Segment> segments { get; set; }
        public string callbackUserName { get; set; }
        public List<string> callbackNumbers { get; set; }
        public DateTime? callbackScheduledTime { get; set; }
        public string scriptId { get; set; }
        public bool? skipEnabled { get; set; }
        public int? timeoutSeconds { get; set; }
        public string dispositionName { get; set; }
        public string peerId { get; set; }
    }

}
