using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace friends_circle.Models
{
    [NotMapped]
    public class FriendWithDistanceViewModel : Friend
    {

        public virtual double distance { get; set; }

    }
}