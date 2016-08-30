namespace friends_circle.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FriendContext : DbContext
    {
        public FriendContext()
            : base("name=FriendConnection")
        {
        }

        public virtual DbSet<Friend> friends { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Friend>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Friend>()
                .Property(e => e.latitude)
                .IsUnicode(false);

            modelBuilder.Entity<Friend>()
                .Property(e => e.longitude)
                .IsUnicode(false);

            modelBuilder.Entity<Friend>()
                .Property(e => e.full_address)
                .IsUnicode(false);

            modelBuilder.Entity<Friend>()
                .Property(e => e.street)
                .IsUnicode(false);
        }
    }
}
