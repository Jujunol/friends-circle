namespace friends_circle.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Friend
    {
        [Key]
        [Column(Order = 0)]
        public int friend_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(25)]
        [DisplayName("Name: ")]
        public string name { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string latitude { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string longitude { get; set; }
    }
}
