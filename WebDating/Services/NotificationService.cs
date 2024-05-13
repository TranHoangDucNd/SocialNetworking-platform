using WebDating.DTOs;
using WebDating.Entities.NotificationEntities;
using WebDating.Interfaces;

namespace WebDating.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;
        public NotificationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<NotificationVM>> GetNewest(int userId, int limit = 20)
        {
            if (limit <= 0)
            {
                limit = 20;
            }
            IEnumerable<Notification> notifications = await _uow.NotificationRepository.GetAllByUserId(userId, limit);
            return notifications.Select(it => new NotificationVM
            {
                Id = it.Id,
                Content = it.Content,
                PostId = it.PostId,
                Status = it.Status,
                CreatedDate = it.CreatedDate,
            });
        }

        public async Task<IEnumerable<NotificationVM>> GetUnread(int userId, int limit = 20)
        {
            if (limit <= 0)
            {
                limit = 20;
            }
            IEnumerable<Notification> notifications = await _uow.NotificationRepository.GetUnreadByUser(userId, limit);
            return notifications.Select(it => new NotificationVM
            {
                Id = it.Id,
                Content = it.Content,
                PostId = it.PostId,
                Status = it.Status,
                CreatedDate = it.CreatedDate,
            });
        }
        public async Task<ResultDto<string>> Delete(int notificationId, int userId)
        {
            var notification = _uow.NotificationRepository.GetById(notificationId);
            if (notification is null)
            {
                return new ErrorResult<string>("Thông báo không tồn tại");
            }
            if (notification.NotifyToUserId != userId)
            {
                return new ErrorResult<string>("Không có quyền xóa");
            }
            _uow.NotificationRepository.Delete(notification);
            bool success = await _uow.Complete();
            return success ? new SuccessResult<string>("Xóa thành công") : new ErrorResult<string>("Thất bại");
        }

        public async Task<ResultDto<string>> MarkAsRead(int notificationId, int userId)
        {
            Entities.NotificationEntities.Notification notification = _uow.NotificationRepository.GetById(notificationId);
            if (notification is null)
            {
                return new ErrorResult<string>("Thông báo không hợp lệ hoặc có thể đã bị xóa");
            }
            if (notification.NotifyToUserId != userId)
            {
                return new ErrorResult<string>("Không có quyền update");
            }
            _uow.NotificationRepository.UpdateStatus(notificationId, Entities.NotificationEntities.NotificationStatus.Read);
            bool success = await _uow.Complete();
            return success ? new SuccessResult<string>("Cập nhật thành công") : new ErrorResult<string>("Thất bại");
        }
    }
}
