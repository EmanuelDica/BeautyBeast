using Microsoft.EntityFrameworkCore;
using BeautyBeastApi.Entities;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace BeautyBeastApi.Data;

public class BeautyBeastContext(DbContextOptions<BeautyBeastContext> options)
                : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Client> Clients { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<ArtistAchievement> ArtistAchievements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        // Inheritance setup for User, Client, and Artist
        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("UserType")
            .HasValue<User>("User")
            .HasValue<Client>("Client")
            .HasValue<Artist>("Artist");

        // Artists
        modelBuilder.Entity<Artist>().HasData(
            new { Id = 1, FullName = "Lea Vinci", Email = "leavinci@gmail.com", ProfilePictureUrl = "lea.jpg", DateJoined = new DateTime(2024, 01, 01), Bio = "Master of PMU", HashedPassword = "artist1Pass", Role = "Artist" },
            new { Id = 2, FullName = "Rachel Hertzler", Email = "rachelhertz@gmail.com", ProfilePictureUrl = "rachel.jpg", DateJoined = new DateTime(2024, 01, 01), Bio = "Make-up Artist", HashedPassword = "artist2Pass", Role = "Artist" },
            new { Id = 3, FullName = "Vivian A", Email = "vivas@gmail.com", ProfilePictureUrl = "viv.jpg", DateJoined = new DateTime(2024, 01, 01), Bio = "Hairdresser", HashedPassword = "artist3Pass", Role = "Artist" },
            new { Id = 4, FullName = "Frida Leon", Email = "fridaleon@gmail.com", ProfilePictureUrl = "frida.jpg", DateJoined = new DateTime(2024, 01, 01), Bio = "Aesthetician", HashedPassword = "artist4Pass", Role = "Artist" }
        );

        // Clients
        modelBuilder.Entity<Client>().HasData(
            new { Id = 5, FullName = "John Doe", Email = "johndoe@gmail.com", ProfilePictureUrl = "john.jpg", DateJoined = new DateTime(2024, 01, 01), HashedPassword = "client1Pass", Role = "Client" },
            new { Id = 6, FullName = "Emma Watson", Email = "emma@gmail.com", ProfilePictureUrl = "emma.jpg", DateJoined = new DateTime(2024, 01, 01), HashedPassword = "client2Pass", Role = "Client" }
        );

        // Artist Achievements
        modelBuilder.Entity<ArtistAchievement>().HasData(
            new { Id = 1, ArtistId = 1, Achievement = "PMU Certification" },
            new { Id = 2, ArtistId = 1, Achievement = "Bridal Makeup Specialist" },
            new { Id = 3, ArtistId = 2, Achievement = "Master Stylist" },
            new { Id = 4, ArtistId = 2, Achievement = "Color Correction Expert" },
            new { Id = 5, ArtistId = 3, Achievement = "Skin Care Specialist" },
            new { Id = 6, ArtistId = 3, Achievement = "Hydrafacial Expert" },
            new { Id = 7, ArtistId = 4, Achievement = "Permanent Makeup Trainer" },
            new { Id = 8, ArtistId = 4, Achievement = "Aesthetic Specialist" }
        );

        // Treatments
        modelBuilder.Entity<Treatment>().HasData(
            new
            { 
                Id = 1, 
                Name = "Microblading", 
                Description = "Semi-permanent eyebrows", 
                PreCareInstructions = "Avoid caffeine before the procedure.", 
                AfterCareInstructions = "Avoid water on the face for 24 hours.", 
                ConsentFormUrl = "consent-form-microblading.pdf", 
                BookingDate = new DateOnly(2024, 03, 10), 
                StartTime = new TimeOnly(14, 00), 
                Duration = new TimeSpan(1, 30, 0), 
                ArtistId = 1 
            },
            new
            { 
                Id = 2, 
                Name = "Evening Glam Makeup", 
                Description = "Makeup for special occasions", 
                PreCareInstructions = "Arrive with a clean face.", 
                AfterCareInstructions = "Remove makeup gently with a cleanser.", 
                ConsentFormUrl = "consent-form-glam-makeup.pdf", 
                BookingDate = new DateOnly(2024, 03, 12), 
                StartTime = new TimeOnly(10, 30), 
                Duration = new TimeSpan(2, 0, 0), 
                ArtistId = 2 
            }
        );
        // Bookings
        modelBuilder.Entity<Booking>().HasData(
            new { Id = 1, ClientId = 5, TreatmentId = 1, BookingDateAndTime = new DateTime(2024, 03, 10, 14, 00, 00), BookingStatus = "Confirmed" },
            new { Id = 2, ClientId = 6, TreatmentId = 2, BookingDateAndTime = new DateTime(2024, 03, 12, 10, 30, 00), BookingStatus = "Pending" }
        );

        // Posts
        modelBuilder.Entity<Post>().HasData(
            new
            {
                Id = 1,
                Description = "My first PMU work!",
                MediaUrl = "pmu-work.jpg",
                DatePosted = new DateTime(2024, 02, 20, 16, 00, 00),
                Likes = 0,
                ArtistId = 1
            },
            new
            {
                Id = 2,
                Description = "Chiaroscuro masterpiece!",
                MediaUrl = "chiaroscuro.jpg",
                DatePosted = new DateTime(2024, 02, 21, 15, 30, 00),
                Likes = 0,
                ArtistId = 2
            }
        );
    }
}