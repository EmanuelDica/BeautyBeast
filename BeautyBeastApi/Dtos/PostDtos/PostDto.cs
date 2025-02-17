using BeautyBeastApi.Dtos.CommentDtos;
namespace BeautyBeastApi.Dtos.PostDtos;

public record class PostDto
(
    int Id,
    string Description,
    List<string> MediaUrls,
    DateTime DatePosted,
    List<CommentDto> Comments,
    int Likes,
    int ArtistId,
    string ArtistName
);