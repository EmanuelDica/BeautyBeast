namespace BeautyBeast.Frontend.Models;

public class User
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public DateTime DateJoined;
    public List<User> Followers { get; set; } = [];
    public List<User> Following { get; set; } = [];
    }

    public class Artist : User
    {
        public string Bio { get; set; } = string.Empty;
        public List<ArtistAchievement> Achievements { get; set; } = new();
        public List<Post> Posts { get; set; } = [];
        public List<Treatment> Treatments { get; set; } = [];
    }

    public class Client : User
    {
        public List<Booking> Bookings { get; set; } = [];
        public List<Status> Statuses { get; set; } = [];
    }