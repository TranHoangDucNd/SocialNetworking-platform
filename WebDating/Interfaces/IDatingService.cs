using WebDating.DTOs;
using WebDating.Entities;

namespace WebDating.Interfaces
{
    public interface IDatingService
    {
        Task<ResultDto<DatingProfileVM>> InitDatingProfile(DatingProfileVM datingProfileVM, string userName); //Cái này của cái khởi tạo profile không liên quan

        //Gửi y/c hẹn hò
        Task<ResultDto<string>> SendDatingRequest(int senderId, int crushId);

        //Xóa y/c hẹn hò đến đối tượng cụ thể
        Task<ResultDto<string>> RemoveWaitingDatingRequest(int senderId, int crushId);
        //Hủy hẹn hò trong trường hợp đã xác nhận hẹn hò
        Task<ResultDto<string>> CancelDating(int userId);
        //Xác nhận hẹn hò
        Task<ResultDto<string>> ConfirmDatingRequest(int userId, int datingRequestId);
        //Từ chối hẹn hò
        Task<ResultDto<string>> DenyDatingRequest(int userId, int datingRequestId);
        Task<ResultDto<DatingRequestVM>> Get(int userId, int datingRequestId);
        Task<ResultDto<DatingRequestVM>> GetDating(int userId);

        //set default profile
        Task CreateProfileAndRandomInterestsForUser(string username);

    }
}
