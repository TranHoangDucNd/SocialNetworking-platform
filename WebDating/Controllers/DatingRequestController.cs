using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebDating.Extensions;
using WebDating.Interfaces;

namespace WebDating.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatingRequestController : ControllerBase
    {
        private readonly IDatingService _datingService;
        public DatingRequestController(IDatingService datingService)
        {
            _datingService = datingService;
        }

        [HttpGet]
        [Route("get-dating")]
        public async Task<IActionResult> GetDating()
        {
            var res = await _datingService.GetDating(User.GetUserId());
            return Ok(res);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _datingService.Get(User.GetUserId(), id);
            return Ok(res);
        }


        [HttpPost]
        [Route("send-dating-request")]
        public async Task<IActionResult> SendDatingRequest(int crushId)
        {
            var res = await _datingService.SendDatingRequest(User.GetUserId(), crushId);
            return Ok(res);
        }

        [HttpPost]
        [Route("remove-dating-request")]
        public async Task<IActionResult> RemoveWaitingDatingRequest(int crushId)
        {
            var res = await _datingService.RemoveWaitingDatingRequest(User.GetUserId(), crushId);
            return Ok(res);
        }

        [HttpPost]
        [Route("cancel-dating")]
        public async Task<IActionResult> CancelDating()
        {
            var res = await _datingService.CancelDating(User.GetUserId());
            return Ok(res);
        }

        [HttpPost]
        [Route("{id}/confirm-dating-request")]
        public async Task<IActionResult> ConfirmDatingRequest(int id)
        {
            var res = await _datingService.ConfirmDatingRequest(User.GetUserId(), id);
            return Ok(res);

        }

        [HttpPost]
        [Route("{id}/deny-dating-request")]
        public async Task<IActionResult> DenyDatingRequest(int id)
        {
            var res = await _datingService.DenyDatingRequest(User.GetUserId(), id);
            return Ok(res);
        }

    }
}
