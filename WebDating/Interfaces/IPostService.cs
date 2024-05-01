using WebDating.DTOs;
using WebDating.DTOs.Post;

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
    }
}
