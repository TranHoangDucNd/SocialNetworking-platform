using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Globalization;
using WebDating.Data.Migrations;
using WebDating.DTOs;
using WebDating.DTOs.Post;
using WebDating.Entities;
using WebDating.Entities.NotificationEntities;
using WebDating.Entities.PostEntities;
using WebDating.Entities.UserEntities;
using WebDating.Interfaces;
using WebDating.SignalR;

namespace WebDating.Services
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPhotoService _photoService;
        private readonly IHubContext<CommentSignalR> _commentHubContext;
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public PostService(IMapper mapper, IUnitOfWork uow, UserManager<AppUser> userManager,
            IPhotoService photoService, IHubContext<CommentSignalR> commentHubContext, IHubContext<NotificationHub> notificationHub)
        {
            _mapper = mapper;
            _uow = uow;
            _userManager = userManager;
            _photoService = photoService;
            _commentHubContext = commentHubContext;
            _notificationHubContext = notificationHub;
        }
        public async Task<ResultDto<PostResponseDto>> Create(CreatePostDto requestDto, string username)
        {
            try
            {
                var user = await _uow.UserRepository.GetUserByUsernameAsync(username);
                Post post = new Post
                {
                    Content = requestDto.Content,
                    UserId = user.Id,
                };

                await _uow.PostRepository.Insert(post);
                await _uow.Complete();

                if (requestDto.Image != null && requestDto.Image.Count > 0)
                {
                    var images = await _photoService.AddRangerPhotoAsync(requestDto.Image);
                    foreach (var image in images)
                    {
                        var img = new ImagePost(post.Id, image.SecureUrl.AbsoluteUri, image.PublicId);
                        await _uow.PostRepository.InsertImagePost(img);
                    }

                }
                bool success = await _uow.Complete();
                if (success)
                {
                    List<int> followerIds = await _uow.LikeRepository.GetAllFollowerId(user.Id);
                    if (followerIds.Count > 0)
                    {
                        List<Notification> notifications = new List<Notification>();
                        foreach (int follower in followerIds)
                        {
                            Notification notification = new Notification()
                            {
                                NotifyFromUserId = user.Id,
                                NotifyToUserId = follower,
                                PostId = post.Id,
                                Type = NotificationType.NewPost,
                                Content = generateNotificatioContent(user.KnownAs, NotificationType.NewPost),
                            };
                            notifications.Add(notification);
                            _uow.NotificationRepository.Insert(notification);
                        }
                        success = await _uow.Complete();
                        if (success)
                        {
                            foreach (Notification notification in notifications)
                            {
                                Task t = sendNotificationData(notification.NotifyToUserId, notification);
                            }
                        }
                    }
                }

                var result = _mapper.Map<PostResponseDto>(post);
                return new SuccessResult<PostResponseDto>(result);
            }
            catch (Exception ex)
            {
                return new ErrorResult<PostResponseDto>(ex.Message);
            }
        }



        public async Task<ResultDto<string>> Delete(int id)
        {
            var post = await _uow.PostRepository.GetById(id);
            _uow.PostRepository.Delete(post);
            await _uow.Complete();
            return new SuccessResult<string>();
        }


        public async Task<ResultDto<PostResponseDto>> Detail(int id)
        {
            var post = await _uow.PostRepository.GetById(id);
            var result = _mapper.Map<PostResponseDto>(post);
            return new SuccessResult<PostResponseDto>(result);
        }

        public async Task<ResultDto<List<PostResponseDto>>> GetAll()
        {
            var result = await _uow.PostRepository.GetAll();
            return new SuccessResult<List<PostResponseDto>>(_mapper.Map<List<PostResponseDto>>(result));
        }


        public async Task<ResultDto<List<PostResponseDto>>> GetMyPost(string name)
        {
            var username = await _userManager.FindByNameAsync(name);
            var myPosts = await _uow.PostRepository.GetMyPost(username.Id);
            var result = _mapper.Map<List<PostResponseDto>>(myPosts);
            return new SuccessResult<List<PostResponseDto>>(result);
        }

        public async Task<ResultDto<UserShortDto>> GetUserShort(string name)
        {
            var username = await _uow.UserRepository.GetUserByUsernameAsync(name);
            return new SuccessResult<UserShortDto>(new UserShortDto()
            {
                Id = username.Id,
                FullName = username.UserName,
                KnownAs = username.KnownAs,
                Image = username.Photos.Select(x => x.Url).FirstOrDefault()
            });
        }

        public async Task<ResultDto<List<UserShortDto>>> GetAllUserInfo()
        {
            var users = await _uow.UserRepository.GetAllUserWithPhotosAsync();
            var listUserShort = users.Select(user => new UserShortDto()
            {
                Id = user.Id,
                FullName = user.UserName,
                KnownAs = user.KnownAs ?? string.Empty,
                Image = user.Photos.Select(x => x.Url).FirstOrDefault() ?? string.Empty
            }).ToList();
            return new SuccessResult<List<UserShortDto>>(listUserShort);
        }

        public async Task<ResultDto<PostResponseDto>> Update(CreatePostDto requestDto, string username)
        {
            try
            {
                Post post = await _uow.PostRepository.GetById(requestDto.Id);

                post.Content = requestDto.Content;
                _uow.PostRepository.Update(post);

                if (requestDto.Image != null && requestDto.Image.Count > 0)
                {
                    await _photoService.DeleteRangerPhotoAsync(post.Images); //delete tren cloud
                    _uow.PostRepository.DeleteImages(post.Images); // delete tren db

                    var images = await _photoService.AddRangerPhotoAsync(requestDto.Image);
                    foreach (var image in images)
                    {
                        var img = new ImagePost(post.Id, image.SecureUrl.AbsoluteUri, image.PublicId);
                        await _uow.PostRepository.InsertImagePost(img);
                    }
                }
                await _uow.Complete();

                var result = _mapper.Map<PostResponseDto>(post);
                return new SuccessResult<PostResponseDto>(result);

            }
            catch (Exception ex)
            {
                return new ErrorResult<PostResponseDto>(ex.Message);
            }
        }

        public async Task<Post> GetById(int postId)
        => await _uow.PostRepository.GetById(postId);


        public async Task<(int Likes, int Comments)> GetLikesAndCommentsCount(int postId)
        {
            var postLikes = await _uow.PostRepository.GetCountPostLikesByPostId(postId);
            var postComments = await _uow.PostRepository.GetCountPostCommentByPostId(postId);

            var likesCount = postLikes;
            var commentsCount = postComments;

            return (likesCount, commentsCount);
        }


        public async Task<ResultDto<List<PostResponseDto>>> AddOrUnLikePost(PostFpkDto postFpk)
        {
            var checkLike = await _uow.PostRepository.GetLikeByMultiId(postFpk.UserId, postFpk.PostId);
            if (checkLike is null)
            {
                PostLike postLike = new()
                {
                    UserId = postFpk.UserId,
                    PostId = postFpk.PostId,
                };
                await _uow.PostRepository.InsertPostLike(postLike);
            }
            else
            {
                _uow.PostRepository.DeletePostLike(checkLike);
            }
            await _uow.Complete();

            return await GetAll();
        }

        public async Task<bool> Report(PostReportDto postReport)
        {
            var check = await _uow.PostRepository.GetReport(postReport.UserId, postReport.PostId);
            if (check is not null)
            {
                check.Report = postReport.Report;
                check.Description = postReport.Description;

                _uow.PostRepository.UpdatePostReport(check);
                await _uow.Complete();
            }
            else
            {
                var report = new PostReportDetail()
                {
                    Checked = false,
                    Description = postReport.Description ?? "",
                    PostId = postReport.PostId,
                    UserId = postReport.UserId,
                    Report = postReport.Report,
                    ReportDate = DateTime.UtcNow
                };

                await _uow.PostRepository.InsertPostReport(report);
                await _uow.Complete();
            }

            return true;
        }

        public async Task<ResultDto<List<PostReportDto>>> GetReport()
        {
            var result = await _uow.PostRepository.GetAllReport();
            return new SuccessResult<List<PostReportDto>>(_mapper.Map<List<PostReportDto>>(result));

        }

        #region Comment
        public ResultDto<List<CommentDto>> GetCommentOfPost(int postId)
        {
            var comments = _uow.CommentRepository.GetByPostId(postId);
            return new SuccessResult<List<CommentDto>>(_mapper.Map<List<CommentDto>>(comments));
        }

        public async Task<ResultDto<string>> DeleteComment(int id)
        {
            _uow.CommentRepository.Delete(id);
            bool success = await _uow.Complete();
            return success ? new SuccessResult<string>("Thành công") : new ErrorResult<string>("Lỗi khi xóa");
        }

        public async Task<ResultDto<string>> UpdateComment(CommentPostDto dto)
        {
            var post = await _uow.PostRepository.GetById(dto.PostId);
            if (post is null)
            {
                return new ErrorResult<string>("Không tìm thấy bài đọc bạn bình luận");
            }
            var comment = _uow.CommentRepository.GetById(dto.Id);
            comment.Content = dto.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            _uow.CommentRepository.Update(comment);
            bool success = await _uow.Complete();

            var comments = await GetComments(comment.PostId);
            await _commentHubContext.Clients.All.SendAsync("ReceiveComment", comments);

            return success ? new SuccessResult<string>("Thành công") : new ErrorResult<string>("Lỗi khi cập nhật");
        }

        public async Task<ResultDto<string>> CreateComment(CommentPostDto dto)
        {
            var post = await _uow.PostRepository.GetById(dto.PostId);
            if (post is null)
            {
                return new ErrorResult<string>("Không tìm thấy bài đọc bạn bình luận, nó có thể đã bị xóa");
            }


            #region New

            NotificationType notificationType = NotificationType.CommentPost;
            int notificationToUserId = post.UserId;
            Comment newComment = new Comment
            {
                UserId = dto.UserId,
                PostId = post.Id,
                Content = dto.Content,
            };



            if (dto.ParentCommentId != 0)
            {
                Comment parentComment = _uow.CommentRepository.GetById(dto.ParentCommentId);
                if (parentComment != null)
                {
                    newComment.ParentId = dto.ParentCommentId;
                    newComment.Level = parentComment.Level + 1;
                    if (newComment.Level > 3)
                    {
                        newComment.Level = 3;
                        //Khi cố tình (hoặc có lỗi xảy ra) mà user vẫn trả lời được 1 comment đang ở level 3 rồi thì sẽ không chuyển nó thành level 4, mà vẫn sẽ là level 3 và coi như 2 comment này đang chung parent Id, nghĩa là cùng reply 1 comment
                        newComment.ParentId = parentComment.ParentId;
                    }
                    notificationType = NotificationType.ReplyComment;
                    notificationToUserId = parentComment.UserId;
                }
            }
            _uow.CommentRepository.Insert(newComment);


            Notification notification = null;
            ///Chỉ khi nào target user và user tạo comment khác nhau thì mới thông báo (không tự thông báo cho chính mình)
            if (dto.UserId != notificationToUserId)
            {
                var currentUser = await _uow.UserRepository.GetUserByIdAsync(dto.UserId);
                if (currentUser != null)
                {
                    notification = new Notification()
                    {
                        NotifyFromUserId = dto.UserId,
                        NotifyToUserId = notificationToUserId,
                        PostId = post.Id,
                        Type = notificationType,
                        Content = generateNotificatioContent(currentUser.KnownAs, NotificationType.CommentPost),
                    };
                    _uow.NotificationRepository.Insert(notification);
                }
            }

            bool success = await _uow.Complete();
            if (success && notification != null)
            {
                Task t = sendNotificationData(notificationToUserId, notification);
            }


            return success ? new SuccessResult<string>("Thành công") : new ErrorResult<string>("Lỗi khi comment");
            #endregion
        }

        /// <summary>
        /// Lấy danh sách tất cả comment (dạng cây) của 1 bài viết theo postId
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<ResultDto<List<CommentVM>>> GetComments(int postId)
        {

            /*
             * Đầu tiên get tất cả comment có postId bằng với postId truyền vào
             * Sắp xếp lại comment theo dạng cây(sử dụng đệ quy - createCommentVM)
             */
            var comments = await _uow.CommentRepository.GetByPostId(postId);
            List<CommentVM> models = createCommentVM(postId, 0, comments, 1);

            return new SuccessResult<List<CommentVM>>(models);
        }

        List<CommentVM> createCommentVM(int postId, int parentCommentId, List<Comment> comments, int level)
        {

            /*
             * Quy định level tối đa của comment là 3, nên khi quá level 3 thì dừng đệ quy
             * - parentCommentId: chỉ định hàm đệ quy này sẽ lấy danh sách comment con của comment nào, nói cách khác là tìm tất cả comment mà có ParentId bằng với parentCommentId truyền vào
             * - comments: danh sách các comments (tất cả level) có liên quan tới bài viết có (postId)
             * - level: chỉ định cấp của các comment sẽ được tạo, mỗi lần gọi đệ quy thì tăng level này nên, biểu thị việc lấy comment cấp thấp hơn (comment con)
             */

            List<CommentVM> items = new List<CommentVM>();
            if (level > 3)
                return items;

            ///Lấy tất cả các comment có ParentId bằng với parentCommentId và level=level ở tham số sau đó duyệt danh sách để tạo ViewModel, trong view model lại đệ quy để tạo child-view modle
            IEnumerable<Comment> commentByLevel = comments.Where(it => it.ParentId == parentCommentId && it.Level == level);
            foreach (Comment cmt in commentByLevel)
            {
                CommentVM item = new CommentVM()
                {
                    Id = cmt.Id,
                    Content = cmt.Content,
                    PostId = postId,
                    UserId = cmt.UserId,
                    ParentCommentId = cmt.ParentId,
                    CreateAt = cmt.CreatedAt.ToString(CultureInfo.InvariantCulture),
                };
                ///Thống kê số lượng action mỗi loại (like, haha....), phần này thực tế không cần join bảng để lấy mà chỉ cần sử dụng .Include để load lên, vì đã thiết lập relationship trong context rồi.
                item.Stats = cmt.ReactionLogs.GroupBy(it => it.ReactionType)
                    .ToDictionary(it => it.Key, it => it.Count());


                ///Lấy tất cả comment con của commnet cmt bằng cách gọi lại chính hàm này (đệ quy), với tham số parentCommentId = cmt.Id, @level thì bằng @level +1
                item.Descendants = createCommentVM(postId, cmt.Id, comments, level + 1);
                items.Add(item);
            }
            return items;
        }

        #endregion

        #region Thả react
        public async Task<ResultDto<string>> ReactComment(ReactionRequest request)
        {
            ReactionLog react = _uow.ReactionLogRepository.GetReactUserByComment(request.UserId, request.TargetId);
            Notification notification = null;
            int notificationToUserId = 0;
            if (react is null)
            {
                Comment comment = _uow.CommentRepository.GetById(request.TargetId);
                if (comment is null)
                {
                    return new ErrorResult<string>("Bình luận đã bị xóa hoặc không hiển thị với bạn");
                }
                AppUser targetUser = await _uow.UserRepository.GetUserByIdAsync(comment.UserId);
                if (targetUser is null)
                {
                    return new ErrorResult<string>("Nội dung không tồn tại");
                }

                react = new ReactionLog
                {
                    UserId = request.UserId,
                    CommentId = comment.Id,
                    ReactionType = request.ReactionType,
                    Target = ReactTarget.Comment,
                };
                _uow.ReactionLogRepository.Insert(react);
                notificationToUserId = comment.UserId;

                AppUser currentUser = await _uow.UserRepository.GetUserByIdAsync(request.UserId);


                /*
                 * Để tạo thông báo realtime đến đúng user thì cần biết user hiện tại đang tương tác đến đối tượng nào, và đối tượng đó do ai tạo ra
                 * Ở đây đã biết user hiện tại đang react đến comment nào? Comment đó do ai tạo ra, từ đó truy ngược được người cần thông báo tới
                 */

                notification = new Notification()
                {
                    NotifyFromUserId = request.UserId,
                    NotifyToUserId = notificationToUserId,
                    CommentId = comment.Id,
                    PostId = comment.PostId,
                    Type = NotificationType.ReactionComment,
                    Content = generateNotificatioContent(currentUser.KnownAs, NotificationType.ReactionComment),
                };
                _uow.NotificationRepository.Insert(notification);
            }
            else
            {
                ///Chỗ này remove bởi đang xử lý chung API, khi người dùng nhấn thả cảm xúc lần đầu thì sẽ tạo mới reactionLog, ngược lại khi gọi hàm này mà người dùng đã tương tác thì coi như là hủy thả cảm xúc, do đó cần remove
                if (react.ReactionType == request.ReactionType)
                    _uow.ReactionLogRepository.Remove(react);
                else
                    react.ReactionType = request.ReactionType;
            }

            bool success = await _uow.Complete();

            if (success && notification != null)
            {
                ///Cần confirm update database success thì mới gửi thông báo đến hub
                Task t = sendNotificationData(notificationToUserId, notification);
            }


            return success ? new SuccessResult<string>("Thành công") : new ErrorResult<string>("Lỗi tương tác cảm xúc bình luận");
        }
        public async Task<ResultDto<string>> ReactPost(ReactionRequest request)
        {
            ReactionLog react = _uow.ReactionLogRepository.GetReactUserByPost(request.UserId, request.TargetId);
            Notification notification = null;
            int notificationToUserId = 0;
            if (react is null)
            {

                Post post = await _uow.PostRepository.GetById(request.TargetId);
                if (post is null)
                {
                    return new ErrorResult<string>("Bài viết đã bị xóa hoặc không hiển thị với bạn");
                }
                AppUser targetUser = await _uow.UserRepository.GetUserByIdAsync(post.UserId);
                if (targetUser is null)
                {
                    return new ErrorResult<string>("Nội dung không tồn tại");
                }
                react = new ReactionLog
                {
                    UserId = request.UserId,
                    PostId = request.TargetId,
                    ReactionType = request.ReactionType,
                    Target = ReactTarget.Post,
                };
                _uow.ReactionLogRepository.Insert(react);


                AppUser currentUser = await _uow.UserRepository.GetUserByIdAsync(request.UserId);
                notificationToUserId = post.UserId;
                notification = new Notification()
                {
                    NotifyFromUserId = request.UserId,
                    NotifyToUserId = notificationToUserId,
                    Type = NotificationType.ReactionPost,
                    Content = generateNotificatioContent(currentUser.KnownAs, NotificationType.ReactionPost),
                    PostId = post.Id,
                };
                _uow.NotificationRepository.Insert(notification);

            }
            else
            {
                if (react.ReactionType == request.ReactionType)
                    _uow.ReactionLogRepository.Remove(react);
                else
                    react.ReactionType = request.ReactionType;
            }
            bool success = await _uow.Complete();
            if (success && notification != null)
            {
                Task t = sendNotificationData(notificationToUserId, notification);
            }
            return success ? new SuccessResult<string>("Thành công") : new ErrorResult<string>("Lỗi tương tác cảm xúc bài viết");
        }

        public async Task<ResultDto<List<ReactionLogVM>>> GetDetailReaction(int targetId, bool isPost)
        {
            List<ReactionLogVM> vms = new List<ReactionLogVM>();
            List<ReactionLog> reactions = isPost ? await _uow.ReactionLogRepository.GetDetailReactionForPost(targetId) : await _uow.ReactionLogRepository.GetDetailReactionForComment(targetId);

            List<AppUser> userCommented = await _uow.UserRepository.GetMany(reactions.Select(it => it.UserId));
            foreach (var react in reactions)
            {
                var user = userCommented.Find(it => it.Id == react.UserId);
                if (user != null)
                {
                    var vm = new ReactionLogVM()
                    {
                        Type = react.ReactionType,
                        DisplayName = Convert.ToString(react.ReactionType),
                        UserFullName = user.KnownAs,
                        UserId = user.Id,
                    };
                    vms.Add(vm);
                }

            }
            return new SuccessResult<List<ReactionLogVM>>() { ResultObj = vms };
        }
        #endregion

        #region Notification
        private string generateNotificatioContent(string fullname, NotificationType notificationType)
        {
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
            }else if(notificationType == NotificationType.NewPost)
            {
                return string.Format("{0} người bạn đang theo dõi vừa đăng một bài đăng mới", fullname);
            }
            return string.Empty;
        }
        #endregion

        #region SignalR
        private async Task sendData(string eventName, int userId, object data)
        {
            await _notificationHubContext.Clients.User(Convert.ToString(userId)).SendAsync(eventName, data);
        }
        private async Task sendNotificationData(int userId, Notification notification)
        {
            await sendData("SendNotification", userId, new
            {
                PostId = notification.PostId,
                CommentId = notification.CommentId,
                Id = notification.Id,
                Content = notification.Content,
                Type = notification.Type,
                Status = notification.Status,
                CreatedDate = notification.CreatedDate,
                From = notification.NotifyFromUserId,
                To = notification.NotifyToUserId,
            });
        }

        #endregion
    }
}
