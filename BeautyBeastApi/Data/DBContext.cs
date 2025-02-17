using Microsoft.EntityFrameworkCore;
using BeautyBeastApi.Entities;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // def inheritance for user client and artist
        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("UserType")
            .HasValue<User>("User")
            .HasValue<Client>("Client")
            .HasValue<Artist>("Artist");

        //declare artists &clients separately
        var artist1 = new Artist { Id = 1, FullName = "Lea Vinci", 
        Email = "leavinci@gmail.com", ProfilePictureUrl = "lea.jpg", 
        DateJoined = new DateTime(2024, 01, 01), Bio = "Master of PMU"};

        var artist2 = new Artist { Id = 2, FullName = "Rachel Hertzler", 
        Email = "rachelhertz@gmail.com", ProfilePictureUrl = "rachel.jpg", 
        DateJoined = new DateTime(2024, 01, 01), Bio = "Make-up Artist" };

        var artist3 = new Artist { Id = 3, FullName = "Vivian A", 
        Email = "vivas@gmail.com", ProfilePictureUrl = "viv.jpg", 
        DateJoined = new DateTime(2024, 01, 01),Bio = "Hairdresser" };

        var artist4 = new Artist { Id = 4, FullName = "Frida Leon",
        Email = "fridaleon@gmail.com", ProfilePictureUrl = "frida.jpg", 
        DateJoined = new DateTime(2024, 01, 01), Bio = "Aesthetician" };

        var client1 = new Client { Id = 5, FullName = "John Doe", 
        Email = "johndoe@gmail.com", ProfilePictureUrl = "john.jpg",
        DateJoined = new DateTime(2024, 01, 01) };

        var client2 = new Client { Id = 6, FullName = "Emma Watson",
        Email = "emma@gmail.com", ProfilePictureUrl = "emma.jpg", 
        DateJoined = new DateTime(2024, 01, 01) };

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

        // add artists separately
        modelBuilder.Entity<Artist>().HasData(
            new { Id = 1, FullName = artist1.FullName, Email = artist1.Email, 
            ProfilePictureUrl = artist1.ProfilePictureUrl, Bio = artist1.Bio },

            new { Id = 2, FullName = artist2.FullName, Email = artist2.Email, 
            ProfilePictureUrl = artist2.ProfilePictureUrl, Bio = artist2.Bio },

            new { Id = 3, FullName = artist3.FullName, Email = artist3.Email, 
            ProfilePictureUrl = artist3.ProfilePictureUrl, Bio = artist3.Bio },

            new { Id = 4, FullName = artist4.FullName, Email = artist4.Email, 
            ProfilePictureUrl = artist4.ProfilePictureUrl, Bio = artist4.Bio }
        );

        //add clients separately
        modelBuilder.Entity<Client>().HasData(
            new { Id = 5, FullName = client1.FullName, Email = client1.Email, 
            ProfilePictureUrl = client1.ProfilePictureUrl },

            new { Id = 6, FullName = client2.FullName, Email = client2.Email, 
            ProfilePictureUrl = client2.ProfilePictureUrl }
        );

        //declare treatments
        var treatments = new List<Treatment>
        {
            // lea treatments
            new() { Id = 1, Name = "Microblading", Description = "Semi-permanent eyebrows", 
            PreCareInstructions = "Avoid caffeine", AfterCareInstructions = "Follow aftercare card", 
            ConsentFormUrl = "microblading-consent.pdf", BookingDate = new DateOnly(2025, 10, 25), 
            StartTime = new TimeOnly(14, 00, 00), Duration = TimeSpan.FromMinutes(90), ArtistId = 1 },

            new() { Id = 2, Name = "Ombre Brows", Description = "Powdered shading brows", 
            PreCareInstructions = "No makeup before session", AfterCareInstructions = "Moisturize daily", 
            ConsentFormUrl = "ombre-brows-consent.pdf", BookingDate = new DateOnly(2025, 11, 02), 
            StartTime = new TimeOnly(10, 30, 00), Duration = TimeSpan.FromMinutes(100), ArtistId = 1 },

            // Rachel treatments
            new() { Id = 3, Name = "Bridal Makeup", Description = "Full bridal makeup", 
            PreCareInstructions = "Clean face before", AfterCareInstructions = "Use oil-free remover", 
            ConsentFormUrl = "bridal-consent.pdf", BookingDate = new DateOnly(2025, 09, 15), 
            StartTime = new TimeOnly(11, 00, 00), Duration = TimeSpan.FromMinutes(120), ArtistId = 2 },

            new() { Id = 4, Name = "Evening Glam Makeup", Description = "Makeup for special occasions", 
            PreCareInstructions = "Moisturize before", AfterCareInstructions = "Use gentle remover",
            ConsentFormUrl = "evening-makeup-consent.pdf", BookingDate = new DateOnly(2025, 12, 05), 
            StartTime = new TimeOnly(18, 00, 00), Duration = TimeSpan.FromMinutes(90), ArtistId = 2 }
        };

        modelBuilder.Entity<Treatment>().HasData(treatments);

        // bookings
        var bookings = new List<Booking>
        {
            new() { Id = 1, ClientId = 5, TreatmentId = 1, 
            BookingDateAndTime = new DateTime(2024, 03, 10, 14, 00, 00) },

            new() { Id = 2, ClientId = 6, TreatmentId = 4, 
            BookingDateAndTime = new DateTime(2024, 03, 12, 10, 30, 00) }
        };


        modelBuilder.Entity<Booking>().HasData(bookings);

        // posts
        var posts = new List<Post>
        {
            new() { Id = 1, Description = "My first PMU work!", 
            MediaUrls = new List<string> { "pmu-work.jpg" }, 
            DatePosted = new DateTime(2024, 02, 20, 16, 00, 00), ArtistId = 1 },

            new() { Id = 2, Description = "Chiaroscuro masterpiece!", 
            MediaUrls = new List<string> { "chiaroscuro.jpg" }, 
            DatePosted = new DateTime(2024, 02, 21, 15, 30, 00), ArtistId = 2 }
        };

        modelBuilder.Entity<Post>().HasData(posts);
    }
}