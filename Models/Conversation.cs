using System.Collections.Generic;
using System;

namespace WebJobGxCGenesys.Models {
    public class Conversation {
        public string conversationId { get; set; }
        public DateTime conversationStart { get; set; }
        public DateTime conversationEnd { get; set; }
        public List<Participant> participants { get; set; }
    }
}
