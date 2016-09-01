using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace friends_circle.Models
{
    public class FriendListViewModel
    {

        public IEnumerable<FriendWithDistanceViewModel> friendList { get; set; }
        public string clientIp { get; set; }
        public string lng { get; set; }
        public string lat { get; set; }
        public string location { get; set; }

    }
}