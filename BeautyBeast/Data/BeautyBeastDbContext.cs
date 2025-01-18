using Microsoft.EntityFrameworkCore;
using BeautyBeastApp.Models;

namespace BeautyBeastApp.Data
{
    public class BeautyBeastDbContext : DbContext
    {
        public BeautyBeastDbContext(DbContextOptions<BeautyBeastDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Artist>("Artist")
                .HasValue<Client>("Client");

             modelBuilder.Entity<Post>()
                .HasOne(p => p.Artist)
                .WithMany(a => a.Posts)
                .HasForeignKey(p => p.ArtistId);

            // Configure Treatments to reference Artists
            modelBuilder.Entity<Treatment>()
                .HasOne(t => t.Artist)
                .WithMany(a => a.Treatments)
                .HasForeignKey(t => t.ArtistId);


            modelBuilder.Entity<Post>()
                .HasOne(p => p.Artist)
                .WithMany(a => a.Posts)
                .HasForeignKey(p => p.ArtistId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Client)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.ClientId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Treatment)
                .WithMany()
                .HasForeignKey(b => b.TreatmentId);

            modelBuilder.Entity<Treatment>()
                .HasOne(t => t.Artist)
                .WithMany(a => a.Treatments)
                .HasForeignKey(t => t.ArtistId);
        }
    }
}