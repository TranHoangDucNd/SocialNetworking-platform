using WebDating.DTOs;
using WebDating.DTOs.Post;
using WebDating.Entities.PostEntities;

namespace WebDating.Interfaces
{
    public interface IPostService
    {
        Task<ResultDto<PostResponseDto>> Create(CreatePostDto requestDto, string name);
        Task<ResultDto<PostResponseDto>> Update(CreatePostDto requestDto, string name);
        Task<ResultDto<PostResponseDto>> Detail(int id);
        Task<ResultDto<List<PostResponseDto>>> GetAll();
        Task<ResultDto<UserShortDto>> GetUserShort(string name);
        Task<ResultDto<List<PostResponseDto>>> GetMyPost(string name);
        Task<ResultDto<string>> Delete(int id);

        //Comment
        Task<ResultDto<List<CommentPostDto>>> GetComment(Post post);
        Task<ResultDto<List<CommentPostDto>>> DeleteComment(int id);
        Task<ResultDto<List<CommentPostDto>>> UpdateComment(CommentPostDto comment);
        Task<ResultDto<List<CommentPostDto>>> CreateComment(CommentPostDto comment, int userId);
        Task<Post> GetById(int postId);

        //Task<int> CountLikes(int postId);
        //Task<int> CountComments(int postId);

        Task<(int Likes, int Comments)> GetLikesAndCommentsCount(int postId);
        Task<ResultDto<List<PostResponseDto>>> AddOrUnLikePost(PostFpkDto postFpk);

        Task<bool> Report(PostReportDto postReport);
        Task<ResultDto<List<PostReportDto>>> GetReport();

    }
}
