using Microsoft.AspNetCore.SignalR;
using WebDating.DTOs;
using WebDating.Entities.NotificationEntities;
using WebDating.Interfaces;
using WebDating.SignalR;

namespace WebDating.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public NotificationService(IUnitOfWork uow, IHubContext<NotificationHub> notificationHub)
        {
            _uow = uow;
            _notificationHub = notificationHub;
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
                UserId = it.NotifyFromUserId,
                Status = it.Status,
                Type = it.Type,
                DatingRequestId = it.DatingRequestId,
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
                UserId = it.NotifyFromUserId,
                Status = it.Status,
                Type = it.Type,
                DatingRequestId = it.DatingRequestId,
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
        //Đánh dấu là đã đọc
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

        #region Notification
        public string GenerateNotificationContent(string fullname, NotificationType notificationType)
        {
            //Post
            if (notificationType == NotificationType.ReactionPost)
            {
                return string.Format("{0} vừa bày tỏ cảm xúc về bài viết của bạn", fullname);
            }
            else if (notificationType == NotificationType.CommentPost)
            {
                return string.Format("{0} vừa bình luận bài viết của bạn", fullname);
            }
            else if (notificationType == NotificationType.ReplyComment)
            {
                return string.Format("{0} vừa trả lời bình luận của bạn", fullname);
            }
            else if (notificationType == NotificationType.ReactionComment)
            {
                return string.Format("{0} vừa bày tỏ cảm xúc về bình luận của bạn", fullname);
            }
            else if (notificationType == NotificationType.NewPost)
            {
                return string.Format("{0} người bạn đang theo dõi vừa đăng một bài đăng mới", fullname);
            }

            //Dating
            else if (notificationType == NotificationType.SentDatingRequest)
            {
                return string.Format("{0} đã gửi yêu cầu hẹn hò với bạn", fullname);
            }
            else if (notificationType == NotificationType.ConfirmedDatingRequest)
            {
                return string.Format("Thật tuyệt, {0} đã đồng ý yêu cầu hẹn hò với bạn", fullname);
            }
            else if (notificationType == NotificationType.DeniedDatingRequest)
            {
                return string.Format("{0} đã từ chối yêu cầu hẹn hò với bạn, đừng buồn hãy kiên trì nhé", fullname);
            }
            else if (notificationType == NotificationType.CancelDating)
            {
                return string.Format("{0} đã hủy hẹn hò với bạn", fullname);
            }
            return string.Empty;
        }
        #endregion

        #region SignalR
        private async Task SendData(string eventName, int userId, object data)
        {
            await _notificationHub.Clients.User(Convert.ToString(userId)).SendAsync(eventName, data);
        }
        public async Task SendNotification(int userId, Notification notification)
        {
            await SendData("SendNotification", userId, new
            {
                PostId = notification.PostId,
                CommentId = notification.CommentId,
                Id = notification.Id,
                Content = notification.Content,
                Type = notification.Type,
                Status = notification.Status,
                CreatedDate = notification.CreatedDate,
                DatingRequestId = notification.DatingRequestId,
                UserId = notification.NotifyFromUserId,
                From = notification.NotifyFromUserId,
                To = notification.NotifyToUserId,
            });
        }
        #endregion
    }
}
