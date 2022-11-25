using PureCloudPlatform.Client.V2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebJobDinersHT1;

namespace WebJobGxCGenesys.Models {
    public class Entity {
        public string id { get; set; }
        public string name { get; set; }
        public Division division { get; set; }
        public Chat chat { get; set; }
        public string department { get; set; }
        public string email { get; set; }
        public List<PrimaryContactInfo> primaryContactInfo { get; set; }
        public List<object> addresses { get; set; }
        public string state { get; set; }
        public string username { get; set; }
        public List<Image> images { get; set; }
        public int version { get; set; }
        public bool acdAutoAnswer { get; set; }
        public string selfUri { get; set; }
        public string title { get; set; }
    }

}
