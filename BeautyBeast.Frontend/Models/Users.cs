namespace BeautyBeast.Frontend.Models;

public class User
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public DateTime DateJoined { get; set; } = DateTime.UtcNow; 
    public required string HashedPassword { get; set; }
    public required string Role { get; set; } 
    public List<User> Followers { get; set; } = new List<User>();
    public List<User> Following { get; set; } = new List<User>();
}

public class Artist : User
{
    public string Bio { get; set; } = string.Empty;
    
    public List<ArtistAchievement> Achievements { get; set; } = new List<ArtistAchievement>();
    
    public List<Post> Posts { get; set; } = new List<Post>();
    
    public List<Treatment> Treatments { get; set; } = new List<Treatment>();
}

public class Client : User
{
    public List<Booking> Bookings { get; set; } = new List<Booking>();
    
    public List<Status> Statuses { get; set; } = new List<Status>();
}