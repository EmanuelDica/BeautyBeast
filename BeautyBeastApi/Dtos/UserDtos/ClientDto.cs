using BeautyBeastApi.Dtos.BookingDtos;
using BeautyBeastApi.Dtos.StatusDtos;

namespace BeautyBeastApi.Dtos.UserDtos;
public record class ClientDto(
    int Id,
    string FullName,
    string Email,
    string? ProfilePictureUrl,
    DateTime DateJoined,
    List<BookingDto> Bookings,
    List<StatusDto> Statuses
) : UserDto(Id, FullName, Email, ProfilePictureUrl, DateJoined);
