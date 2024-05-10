using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using WebDating.DTOs;
using WebDating.DTOs.Post;
using WebDating.Entities;
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

        public PostService(IMapper mapper, IUnitOfWork uow, UserManager<AppUser> userManager,
            IPhotoService photoService, IHubContext<CommentSignalR> commentHubContext)
        {
            _mapper = mapper;
            _uow = uow;
            _userManager = userManager;
            _photoService = photoService;
            _commentHubContext = commentHubContext;
        }
        public async Task<ResultDto<PostResponseDto>> Create(CreatePostDto requestDto, string name)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(name);
                var post = new Post() { Content = requestDto.Content };
                post.CreatedAt = DateTime.Now;
                post.IsDeleted = false;
                post.UserId = user.Id;

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
                    await _uow.Complete();

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
                FullName = username.KnownAs,
                Image = username.Photos.Select(x => x.Url).FirstOrDefault()
            });
        }

        public async Task<ResultDto<PostResponseDto>> Update(CreatePostDto requestDto, string name)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(name);
                var post = await _uow.PostRepository.GetById(requestDto.Id);

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

                    await _uow.Complete();
                }

                var result = _mapper.Map<PostResponseDto>(post);
                return new SuccessResult<PostResponseDto>(result);

            }
            catch (Exception ex)
            {
                return new ErrorResult<PostResponseDto>(ex.Message);
            }
        }
        public async Task<ResultDto<List<CommentPostDto>>> CreateComment(CommentPostDto comment, int userId)
        {
            var post = await _uow.PostRepository.GetById(comment.PostId);
            if (post == null)
            {
                return new
                    ErrorResult<List<CommentPostDto>>("Không tìm thấy bài đọc bạn bình luận, nó có thể đã bị xóa");
            }


            var postComment = new PostComment()
            {
                PostId = post.Id,
                UserId = comment.UserId,
                Content = comment.Content
            };
            postComment.UpdatedAt = DateTime.UtcNow;

            await _uow.PostRepository.InsertComment(postComment);
            await _uow.Complete();

            var comments = await GetComment(post);
            await _commentHubContext.Clients.All.SendAsync("ReceiveComment", comments);
            return comments;

        }

        public async Task<ResultDto<List<CommentPostDto>>> GetComment(Post post)
        {
            var users = await _uow.UserRepository.GetAll();
            var postComments = await _uow.PostRepository.GetCommentsByPostId(post.Id);
            return new SuccessResult<List<CommentPostDto>>(_mapper.Map<List<CommentPostDto>>(postComments));
        }

        public async Task<Post> GetById(int postId)
        => await _uow.PostRepository.GetById(postId);

        public async Task<ResultDto<List<CommentPostDto>>> UpdateComment(CommentPostDto comment)
        {
            var postComment = await _uow.PostRepository.GetCommentById(comment.Id);
            var post = await _uow.PostRepository.GetById(comment.PostId);
            if (postComment == null)
            {
                return new ErrorResult<List<CommentPostDto>>("Không tìm thấy bài đọc bạn bình luận");
            }
            postComment.Content = comment.Content;
            postComment.UpdatedAt = DateTime.UtcNow;

            _uow.PostRepository.UpdateComment(postComment);
            await _uow.Complete();

            var comments = await GetComment(post);
            await _commentHubContext.Clients.All.SendAsync("ReceiveComment", comments);

            return comments;
        }
        public async Task<ResultDto<List<CommentPostDto>>> DeleteComment(int id)
        {
            var comment = await _uow.PostRepository.GetCommentById(id);
            var post = await _uow.PostRepository.GetById(comment.PostId);

            if (comment == null)
            {
                return new ErrorResult<List<CommentPostDto>>("Không tìm thấy bình luận");
            }

            _uow.PostRepository.DeleteComment(comment);
            await _uow.Complete();

            var comments = await GetComment(post);
            await _commentHubContext.Clients.All.SendAsync("ReceiveComment", comments);
            return comments;
        }

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
        #endregion
    }
}
