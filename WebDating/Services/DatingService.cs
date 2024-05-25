using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebDating.DTOs;
using WebDating.Entities.MessageEntities;
using WebDating.Entities.NotificationEntities;
using WebDating.Entities.ProfileEntities;
using WebDating.Entities.UserEntities;
using WebDating.Interfaces;

namespace WebDating.Services
{
    public class DatingService : IDatingService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly INotificationService _notificationService;

        public DatingService(IMapper mapper, IUnitOfWork uow, INotificationService notificationService)
        {
            _mapper = mapper;
            _uow = uow;
            _notificationService = notificationService;
        }
        #region Xử lý yêu cầu hẹn hò và xác nhận/ từ chối yêu cầu
        public async Task<ResultDto<string>> CancelDating(int userId)
        {
            DatingRequest existingDating = await _uow.DatingRequestRepository.GetIfConfirmedRelationship(userId);
            if(existingDating is null)
            {
                return new ErrorResult<string>() { Message = "Bạn đang không trong mối quan hệ nào cả" };
            }
            AppUser currentUser = await _uow.UserRepository.GetUserByIdAsync(userId);
            int notifyToUserId = existingDating.SenderId == userId ? existingDating.CrushId : existingDating.SenderId;
            Notification notification = new Notification()
            {
                NotifyFromUserId = userId,
                NotifyToUserId = notifyToUserId,
                PostId = null,
                DatingRequestId = null,
                Type = NotificationType.CancelDating,
                Content = _notificationService.GenerateNotificationContent(currentUser.KnownAs, NotificationType.CancelDating)
            };

            _uow.NotificationRepository.Insert(notification);

            //xóa hết notification
            _uow.NotificationRepository.RemoveAllByDateId(existingDating.Id);
            _uow.DatingRequestRepository.Remove(existingDating);

            bool success = await _uow.Complete();
            if (success)
            {
                _ = _notificationService.SendNotification(notifyToUserId, notification);
                return new SuccessResult<string>() { Message = "Bạn đã hủy hẹn hò thành công" };
            }
            return new ErrorResult<string>() { Message = "Có lỗi khi hủy yêu cầu, vui lòng thử lại" };
        }
        public async Task<ResultDto<string>> DenyDatingRequest(int userId, int datingRequestId)
        {
            DatingRequest datingRequest = _uow.DatingRequestRepository.Get(datingRequestId);
            if(datingRequest is null || datingRequest.CrushId != userId)
            {
                return new ErrorResult<string>() { Message = "Yêu cầu hẹn hò không hợp lệ hoặc có thể đã bị xóa" };
            }

            datingRequest.Status = DatingStatus.Denied;
            datingRequest.ConfirmedDate = DateTime.Now;
            AppUser currentUser = await _uow.UserRepository.GetUserByIdAsync(userId);

            Notification notification = new Notification()
            {
                NotifyFromUserId = userId,
                NotifyToUserId = datingRequest.SenderId,
                DatingRequestId = datingRequestId,
                Type = NotificationType.DeniedDatingRequest,
                Content = _notificationService.GenerateNotificationContent(currentUser.KnownAs, NotificationType.DeniedDatingRequest)
            };

            _uow.NotificationRepository.Insert(notification);

            bool success = await _uow.Complete();

            if (success)
            {
                _ = _notificationService.SendNotification(datingRequest.SenderId, notification);
                return new SuccessResult<string>() { Message = "Bạn đã từ chối yêu cầu hẹn hò thành công" };
            }
            return new ErrorResult<string>() { Message = "Có lỗi khi thao tác, vui lòng thử lại" };
        }
        public async Task<ResultDto<string>> RemoveWaitingDatingRequest(int senderId, int crushId)
        {
            DatingRequest waitingRequest = await _uow.DatingRequestRepository.GetWaitingRequest(senderId, crushId);
            if (waitingRequest == null)
            {
                return new ErrorResult<string>() { Message = "Bạn chưa gửi yêu cầu hẹn hò nào với người này" };
            }

            _uow.NotificationRepository.RemoveAllByDateId(waitingRequest.Id);
            _uow.DatingRequestRepository.Remove(waitingRequest);
            bool success = await _uow.Complete();
            if (success)
            {
                return new SuccessResult<string>() { Message = "Đã xóa yêu cầu hẹn hò" };
            }
            return new ErrorResult<string>() { Message = "Có lỗi khi gửi xóa, vui lòng thử lại" };
        }

        public async Task<ResultDto<DatingRequestVM>> Get(int userId, int datingRequestId)
        {
            DatingRequest datingRequest = _uow.DatingRequestRepository.Get(datingRequestId);
            if(datingRequest is null || (datingRequest.SenderId != userId && datingRequest.CrushId != userId))
            {
                return new ErrorResult<DatingRequestVM>() { Message = "Yêu cầu hẹn hò không hợp lệ" };
            }
        

            AppUser sender = await _uow.UserRepository.GetUserByIdAsync(datingRequest.SenderId);
            AppUser crush = await _uow.UserRepository.GetUserByIdAsync(datingRequest.CrushId);
            DatingRequestVM vm = DatingRequestVM.CreateMap(datingRequest);

            vm.CrushName = crush.KnownAs;
            vm.CrushAvatar = crush.Photos.FirstOrDefault(it => it.IsMain)?.Url;
            vm.SenderName = sender.KnownAs;
            vm.SenderAvatar = sender.Photos.FirstOrDefault(it => it.IsMain)?.Url;

            return new SuccessResult<DatingRequestVM>(vm) { Message = "Thành công" };
        }

        public async Task<ResultDto<DatingRequestVM>> GetDating(int userId)
        {
            DatingRequest existingDating = await _uow.DatingRequestRepository.GetIfConfirmedRelationship(userId);
            if(existingDating is null)
            {
                return new SuccessResult<DatingRequestVM>() { Message = "Bạn đang không trong mối quan hệ nào cả" };
            }
            DatingRequestVM vm = DatingRequestVM.CreateMap(existingDating);
            AppUser sender = await _uow.UserRepository.GetUserByIdAsync(existingDating.SenderId);
            AppUser crush = await _uow.UserRepository.GetUserByIdAsync(existingDating.CrushId);

            vm.CrushName = crush.KnownAs;
            vm.SenderName = sender.KnownAs;
            vm.CrushAvatar = crush.Photos.FirstOrDefault(it => it.IsMain)?.Url;
            vm.SenderAvatar = sender.Photos.FirstOrDefault(it => it.IsMain)?.Url;

            return new SuccessResult<DatingRequestVM>(vm) { Message = "Thành công" };
        }
       
        public async Task<ResultDto<string>> SendDatingRequest(int senderId, int crushId)
        {
            AppUser crush = await _uow.UserRepository.GetUserByIdAsync(crushId);
            if(crush is null)
            {
                return new ErrorResult<string>() { Message = "Crush không hợp lệ, có thể người dùng đã xóa tài khoản" };
            }
            DatingRequest existWaitingRequest = await _uow.DatingRequestRepository.GetIfExistWaiting(senderId, crushId);
            if(existWaitingRequest != null)
            {
                if(existWaitingRequest.SenderId == senderId)
                {
                    return new ErrorResult<string>() { Message = "Bạn đã gửi yêu cầu hẹn hò đến người này, vui lòng chờ phản hồi" };
                }
                else
                {
                    return new ErrorResult<string>() { Message = "Người này đã gửi cho bạn một yêu cầu hẹn hò trước đó, vui lòng xác nhận" };
                }
            }

            DatingRequest existConfirmedRelationship1 = await _uow.DatingRequestRepository.GetIfConfirmedRelationship(crushId);
            if(existConfirmedRelationship1 != null)
            {
                return new ErrorResult<string>() { Message = "Crush đang trong một mối quan hệ khác, vui lòng không lụy tình" };
            }
            DatingRequest existConfirmedRelationship2 = await _uow.DatingRequestRepository.GetIfConfirmedRelationship(senderId);
            if (existConfirmedRelationship2 != null)
            {
                return new ErrorResult<string>() { Message = "Bạn đã ở trong một mối quan hệ, xin đừng bắt cá hai tay!" };
            }

            DatingRequest existDenied = _uow.DatingRequestRepository.GetIfDeniedBefore(crushId);
            if(existDenied != null)
            {
                return new ErrorResult<string>() { Message = "Crush từ chối yêu cầu hẹn hò trước đó, vui lòng chờ trước khi gửi yêu cầu tiếp theo" };
            }

            ///Xóa hết những yêu cầu mà user hiện tại đã từ chối crushId
            _uow.DatingRequestRepository.RemoveIfMeDenied(senderId, crushId);

            AppUser currentUser = await _uow.UserRepository.GetUserByIdAsync(senderId);
            DatingRequest datingRequest = new DatingRequest()
            {
                CrushId = crushId,
                SenderId = senderId,
                Status = DatingStatus.Waiting,
            };

            var datingRequestId = _uow.DatingRequestRepository.InsertAndGetId(datingRequest);

            Notification notification = new Notification()
            {
                NotifyFromUserId = senderId,
                NotifyToUserId = crushId,
                PostId = null,
                Type = NotificationType.SentDatingRequest,
                DatingRequestId = datingRequestId,
                Content = _notificationService.GenerateNotificationContent(currentUser.KnownAs, NotificationType.SentDatingRequest)
            };

            _uow.NotificationRepository.Insert(notification);

            bool success = await _uow.Complete();

            if (success)
            {
                _ = _notificationService.SendNotification(crushId, notification);
                return new SuccessResult<string> { Message = "Đã gửi yêu cầu hẹn hò thành công tới crush" };
            }
            return new ErrorResult<string> { Message = "Có lỗi khi gửi yêu cầu, vui lòng thử lại" };

        }
        //confirm thì currentuser = crush
        public async Task<ResultDto<string>> ConfirmDatingRequest(int userId, int datingRequestId)
        {
            DatingRequest datingRequest = _uow.DatingRequestRepository.Get(datingRequestId);
            if(datingRequest is null || datingRequest.CrushId != userId)
            {
                return new ErrorResult<string>() { Message = "Yêu cầu hẹn hò không hợp lệ hoặc có thể đã bị xóa" };
            }
            datingRequest.Status = DatingStatus.Confirmed;
            datingRequest.ConfirmedDate = DateTime.Now;
            AppUser currentUser = await _uow.UserRepository.GetUserByIdAsync(userId);

            ///Xóa hết những yêu cầu hẹn hò của cả sender và crush tạo ra
            _uow.DatingRequestRepository.RemoveAllWaitingRequest(userId, datingRequestId);
            _uow.DatingRequestRepository.RemoveAllWaitingRequest(datingRequest.SenderId, datingRequestId);

            Notification notification = new Notification()
            {
                NotifyFromUserId = userId,
                NotifyToUserId = datingRequest.SenderId,
                DatingRequestId = datingRequestId,
                Type = NotificationType.ConfirmedDatingRequest,
                Content = _notificationService.GenerateNotificationContent(currentUser.KnownAs, NotificationType.ConfirmedDatingRequest)
            };
            _uow.NotificationRepository.Insert(notification);

            bool success = await _uow.Complete();

            if (success)
            {
                _ = _notificationService.SendNotification(datingRequest.SenderId, notification);
                return new SuccessResult<string>() { Message = "Bạn đã xác nhận hẹn hò thành công" };
            }
            return new ErrorResult<string>() { Message = "Có lỗi khi xác nhận, vui lòng thử lại" };
        }

        #endregion



        //Tạo mới profile 
        public async Task<ResultDto<DatingProfileVM>> InitDatingProfile(DatingProfileVM datingProfileVM, string userName)
        {
            try
            {
                var dating = _mapper.Map<DatingProfile>(datingProfileVM);
                var user = await _uow.UserRepository.GetUserByUsernameAsync(userName);
                dating.UserId = user.Id;
                //Lưu sở thích của userId vào bảng datingprofile
                var result = await _uow.DatingRepository.Insert(dating);
                await _uow.Complete();
                //Lưu hết r set user update = true

                user.IsUpdatedDatingProfile = true;

                _uow.UserRepository.UpdateUser(user);
                _uow.CompleteNotAsync();

                await _uow.DatingRepository.InsertUserInterest(dating.UserInterests, dating.Id);

                return new SuccessResult<DatingProfileVM>(result);
            }
            catch (Exception ex)
            {
                return new ErrorResult<DatingProfileVM>(ex.Message);
            }

        }

    }
}
