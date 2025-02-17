using System.ComponentModel.DataAnnotations;

namespace BeautyBeastFrontend.Dtos;

public record class ClientDto(
    int Id,
    string FullName,
    string Email,
    string? ProfilePictureUrl,
    DateTime DateJoined,
    List<BookingDto> Bookings,
    List<StatusDto> Statuses
) : UserDto(Id, FullName, Email, ProfilePictureUrl, DateJoined);

public record class CreateClientDto(
    [Required][StringLength(50)] string FullName,
    [Required][StringLength(30)] string Email,
    string ProfilePictureUrl
) : CreateUserDto(FullName, Email, ProfilePictureUrl);

public record class EditClientDto(
    [Required][StringLength(50)]string FullName,
    [Required][StringLength(30)]string Email,
    string? ProfilePictureUrl
);