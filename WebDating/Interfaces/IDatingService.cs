using WebDating.DTOs;
using WebDating.Entities;

namespace WebDating.Interfaces
{
    public interface IDatingService
    {
        Task<ResultDto<DatingProfileVM>> InitDatingProfile(DatingProfileVM datingProfileVM, string userName);


        /// <summary>
        /// Gửi yêu cầu hẹn hò
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="crushId"></param>
        /// <returns></returns>
        Task<ResultDto<string>> SendDatingRequest(int senderId, int crushId);


        /// <summary>
        /// Xóa yêu cầu hẹn hò đến đối tượng cụ thể
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="crushId"></param>
        /// <returns></returns>
        Task<ResultDto<string>> RemoveWaitingDatingRequest(int senderId, int crushId);

        /// <summary>
        /// HỦy hẹn hò (trong trường hợp đã xác nhận hẹn hò)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResultDto<string>> CancelDating(int userId);

        /// <summary>
        /// Xác nhận hẹn hò
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="datingRequestId"></param>
        /// <returns></returns>

        Task<ResultDto<string>> ConfirmDatingRequest(int userId, int datingRequestId);

        /// <summary>
        /// Từ chối hẹn hò
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="datingRequestId"></param>
        /// <returns></returns>

        Task<ResultDto<string>> DenyDatingRequest(int userId, int datingRequestId);


        Task<ResultDto<DatingRequestVM>> Get(int userId, int datingRequestId);

        Task<ResultDto<DatingRequestVM>> GetDating(int userId);
    }
}
