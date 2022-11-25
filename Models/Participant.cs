using System.Collections.Generic;

namespace WebJobGxCGenesys.Models
{
    public class Participant {
        public string participantId { get; set; }
        public string participantName { get; set; }
        public string purpose { get; set; }
        public List<Session> sessions { get; set; }
        public string userId { get; set; }
    }

}
