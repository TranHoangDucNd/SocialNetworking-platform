using System.Threading.Tasks;
using WebDating.DTOs;
using WebDating.DTOs.Post;
using WebDating.Entities.PostEntities;

namespace WebDating.Interfaces
{
    public interface IPostService
    {
        Task<ResultDto<PostResponseDto>> Create(CreatePostDto requestDto, string username);
        Task<ResultDto<PostResponseDto>> Update(CreatePostDto requestDto, string username);
        Task<ResultDto<PostResponseDto>> Detail(int id);
        Task<ResultDto<List<PostResponseDto>>> GetAll();
        Task<ResultDto<UserShortDto>> GetUserShort(string name);
        Task<ResultDto<List<UserShortDto>>> GetAllUserInfo();
        Task<ResultDto<List<PostResponseDto>>> GetMyPost(string name);
        Task<ResultDto<string>> Delete(int id);

        //Comment
        //Task<ResultDto<List<CommentPostDto>>> GetComment(Post post);


        #region New 10/5/2024
        Task<ResultDto<List<CommentVM>>> GetComments(int postId);
        Task<ResultDto<string>> DeleteComment(int id);
        Task<ResultDto<string>> UpdateComment(CommentPostDto dto);
        Task<ResultDto<string>> CreateComment(CommentPostDto dto);

        /// <summary>
        /// Tương tác cảm xúc trên comment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultDto<string>> ReactComment(ReactionRequest request);
        /// <summary>
        /// Tương tác cảm xúc trên post
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultDto<string>> ReactPost(ReactionRequest request);

        Task<ResultDto<List<ReactionLogVM>>> GetDetailReaction(int targetId, bool isPost);
        #endregion



        //Task<ResultDto<List<CommentPostDto>>> DeleteComment(int id);
        //Task<ResultDto<List<CommentPostDto>>> UpdateComment(CommentPostDto comment);
        //Task<ResultDto<List<CommentPostDto>>> CreateComment(CommentPostDto comment);
        Task<Post> GetById(int postId);

        //Task<int> CountLikes(int postId);
        //Task<int> CountComments(int postId);

        Task<(int Likes, int Comments)> GetLikesAndCommentsCount(int postId);
        Task<ResultDto<List<PostResponseDto>>> AddOrUnLikePost(PostFpkDto postFpk);

        Task<bool> Report(PostReportDto postReport);
        Task<ResultDto<List<PostReportDto>>> GetReport();

    }
}
