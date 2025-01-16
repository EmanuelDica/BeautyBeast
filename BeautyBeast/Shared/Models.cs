namespace BeautyBeastApp.Models
{
    // Base User class
    public abstract class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public DateTime DateJoined { get; set; } = DateTime.UtcNow;

        public string PasswordHash { get; set; } = string.Empty;  // Hashed password
        public string PasswordSalt { get; set; } = string.Empty;  // Random salt for additional security
    }

    // Artist class inherits User
    public class Artist : User
    {
        public string Bio { get; set; } = string.Empty;
        public List<string> Achievements { get; set; } = new();
        public List<Post> Posts { get; set; } = new();
        public List<Treatment> Treatments { get; set; } = new();
    }

    // Client class inherits User
    public class Client : User
    {
        public List<Booking> Bookings { get; set; } = new();
        public List<Status> Statuses { get; set; } = new();
    }

    // Post class
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public List<string> MediaUrls { get; set; } = new();
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
        public List<Status> Statuses { get; set; } = new();
        public int ArtistId { get; set; }
        public Artist? Artist { get; set; }
    }

    // Status class
    public class Status
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime DateCommented { get; set; } = DateTime.UtcNow;
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }

    // Treatment class
    public class Treatment
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PreCareInstructions { get; set; } = string.Empty;
        public string AfterCareInstructions { get; set; } = string.Empty;
        public string ConsentFormUrl { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public int ArtistId { get; set; }
        public Artist? Artist { get; set; }
    }

    // Booking class
    public class Booking
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public int TreatmentId { get; set; }
        public Treatment? Treatment { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = "Pending";
    }
}