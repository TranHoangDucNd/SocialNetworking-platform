using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebDating.DTOs;
using WebDating.DTOs.Post;
using WebDating.Entities.PostEntities;
using WebDating.Extensions;
using WebDating.Interfaces;
using WebDating.Utilities;

namespace WebDating.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [Authorize]
        [HttpPost]
        [Route("create-post")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto requestDto)
        {

            var result = await _postService.Create(requestDto, User.Identity.Name);
            return Ok(result);
        }

        [HttpGet("MyPost")]
        [Authorize]
        public async Task<IActionResult> GetMyPost()
        {
            var result = await _postService.GetMyPost(User.Identity.Name);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdatePost([FromForm] CreatePostDto requestDto)
        {
            var result = await _postService.Update(requestDto, User.Identity.Name);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _postService.Detail(id);
            return Ok(result);
        }

        [HttpGet("UserShort")]
        public async Task<IActionResult> GetUserInfo()
        {
            var result = await _postService.GetUserShort(User.Identity.Name);
            return Ok(result);
        }
        
        [HttpGet("AllUserShorts")]
        public async Task<IActionResult> GetAllUserInfo()
        {
            var result = await _postService.GetAllUserInfo();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPost()
        {
            var result = await _postService.GetAll();
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _postService.Delete(id);
            return Ok(result);
        }

        #region New 10/5/2024
        [HttpPost]
        [Route("Chat")]
        [Route("create-comment")]
        [Authorize]
        public async Task<IActionResult> CreateComment(CommentPostDto comment)
        {
            comment.UserId = User.GetUserId();
            var result = await _postService.CreateComment(comment);
            return Ok(result);
        }
        [HttpGet]
        [Route("Chat")]
        [Route("get-comments-of-post")]
        public async Task<IActionResult> GetComments(int postId)
        {
            //var post = await _postService.GetById(postId);
            var result = await _postService.GetComments(postId);
            return Ok(result);
        }
        [Authorize]
        [HttpPut]
        [Route("Chat")]
        [Route("update-comment")]
        public async Task<IActionResult> UpdateComment(CommentPostDto comment)
        {
            var id = User.GetUserId();
            if (!id.Equals(comment.UserId))
            {
                return BadRequest();
            }
            var result = await _postService.UpdateComment(comment);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete]
        [Route("Chat")]
        [Route("delete-comment")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var result = await _postService.DeleteComment(commentId);
            return Ok(result);
        }


        [Authorize]
        [HttpPost]
        [Route("update-comment-reaction")]
        public async Task<IActionResult> UpdateCommentReaction(ReactionRequest request)
        {
            request.UserId = User.GetUserId();
            var res = await _postService.ReactComment(request);
            return Ok(res);
        }

        [Authorize]
        [HttpPost]
        [Route("update-post-reaction")]
        public async Task<IActionResult> UpdatePostReaction(ReactionRequest request)
        {
            request.UserId = User.GetUserId();
            var res = await _postService.ReactPost(request);
            return Ok(res);
        }


        [HttpGet]
        [Route("get-detail-reaction")]
        public async Task<IActionResult> GetDetailReaction(int targetId)
        {
            var res = await _postService.GetDetailReaction(targetId);
            return Ok(res);
        }


        #endregion

        [HttpGet("Chat/NumberComment")]
        public async Task<IActionResult> GetCommentPost(int postId)
        {
            //var post = await _postService.GetById(postId);
            var result = await _postService.GetComments(postId);
            var numberResult = new SuccessResult<int>(result.ResultObj.Count);
            return numberResult is null ? BadRequest(numberResult) : Ok(numberResult);
        }


        [HttpGet("LikesAndComments/{postId}")]
        [Authorize]
        public async Task<IActionResult> GetLikesAndCommentsCount(int postId)
        {
            var (likesCount, commentsCount) = await _postService.GetLikesAndCommentsCount(postId);
            return Ok(new { Likes = likesCount, Comments = commentsCount });
        }


        [HttpPost("Like")]
        public async Task<IActionResult> AddLike(PostFpkDto postFpk)
        {
            var result = await _postService.AddOrUnLikePost(postFpk);
            return Ok(result);
        }


        [HttpGet("ContentReport")]
        public IActionResult GetContentReport()
        {
            var reesult = Utils.GetAllAccountType<Report>();
            return Ok(reesult);
        }


        [HttpPost("Report")]
        public async Task<IActionResult> Report([FromForm] PostReportDto postReport)
        {
            var result = await _postService.Report(postReport);
            return Ok(result);
        }

        [HttpGet("Report")]
        public async Task<IActionResult> GetReport()
        {
            var result = await _postService.GetReport();
            return Ok(result);
        }
    }
}
