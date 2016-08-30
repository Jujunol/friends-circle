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
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column(Order = 0)]
        public virtual int friend_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(25)]
        [DisplayName("Name: ")]
        public virtual string name { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        [DisplayName("Street Address: ")]
        public virtual string street { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public virtual string latitude { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public virtual string longitude { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(100)]
        public virtual string full_address { get; set; }

    }
}
