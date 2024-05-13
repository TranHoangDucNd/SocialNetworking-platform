using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Net.WebSockets;
using WebDating.Entities.ProfileEntities;
using WebDating.Extensions;
using WebDating.Interfaces;

namespace WebDating.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {

        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("get-newest")]
        public async Task<IActionResult> GetNewest(int limit = 40)
        {
            var notifications = await _service.GetNewest(User.GetUserId(), limit);
            return Ok(notifications);
        }

        [HttpGet]
        [Route("get-unread")]
        public async Task<IActionResult> GetUnRead(int limit = 40)
        {
            var notifications = await _service.GetUnread(User.GetUserId(), limit);
            return Ok(notifications);
        }

        [HttpPost]
        [Route("mark-as-read")]
        public async Task<IActionResult> MarkNotificationRead(int notificationId)
        {
            var res = await _service.MarkAsRead(notificationId, User.GetUserId());
            return Ok(res);
        }
    }
}
