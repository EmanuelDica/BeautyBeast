namespace BeautyBeastApi.Dtos.CommentDtos;

public record class CommentDto(
    int Id,
    string TheComment,
    int Likes,
    int PostId 
);