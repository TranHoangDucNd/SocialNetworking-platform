using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebDating.DTOs.Post;
using WebDating.Extensions;
using WebDating.Interfaces;

namespace WebDating.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        [Authorize]
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
        public async Task<IActionResult> GetUserInfor()
        {
            var result = await _postService.GetUserShort(User.Identity.Name);
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

    }
}
