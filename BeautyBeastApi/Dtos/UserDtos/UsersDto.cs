namespace BeautyBeastApi.Dtos.UserDtos;

public record class UserDto( 
    int Id,
    string FullName,
    string Email,
    string? ProfilePictureUrl,
    DateTime DateJoined
);