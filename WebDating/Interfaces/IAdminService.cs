using WebDating.DTOs;
using WebDating.DTOs.Post;

namespace WebDating.Interfaces
{
    public interface IAdminService
    {
        Task<ResultDto<List<PostReportAdminDto>>> GetPostReports();
        Task<ResultDto<List<PostReportAdminDto>>> DeletePostReport(int postId);
        Task<ResultDto<ShowPostAdminDto>> GetPost(int postId);
    }
}
