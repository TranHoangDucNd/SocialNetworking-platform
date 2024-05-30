using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDating.Data;
using WebDating.DTOs;
using WebDating.Entities.UserEntities;
using WebDating.Helpers;
using WebDating.Interfaces;

namespace WebDating.Controllers
{
    public class AdminController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAdminService _adminService;

        public AdminController(UserManager<AppUser> userManager, IAdminService adminService)
        {
            _userManager = userManager;
            _adminService = adminService;
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles([FromQuery] string userName)
        {
            var query = _userManager
             .Users.AsQueryable();

            if (!string.IsNullOrEmpty(userName))
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null) return NotFound("Không tìm thấy user!");

                query = query.Where(x => x.UserName.ToLower() == userName.ToLower());
            }

            var result = await query.Select(x => new
            {
                x.Id,
                UserName = x.UserName,
                Roles = x.UserRoles.Select(x => x.Role.Name).ToList(),
                KnownAs = x.KnownAs,
                PhotoUrl = x.Photos.FirstOrDefault(x => x.IsMain).Url
            }).ToListAsync();
        
            return Ok(result);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            if (string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role!");
            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles)); // loại bỏ các vai trò mà người dùng đã có sẵn, chỉ giữ lại các vai trò mới mà người dùng chưa có

            if (!result.Succeeded) return BadRequest("Failed to add to roles");


            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles)); // loại bỏ các vai trò 2 cái đã có chỉ giữ lại vai trò mà userrole còn và xóa nó đi

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotoForModerator()
        {
            return Ok("Admins or moderator can see this");
        }

        [Authorize(Policy = "ManageReport")]
        [HttpGet("get-reports")]
        public async Task<ActionResult> GetPostReports()
        {
            var result = await _adminService.GetPostReports();
            return Ok(result);
        }

        [Authorize(Policy = "ManageReport")]
        [HttpGet("get-detail-post-report/{postId}")]
        public async Task<ActionResult> GetDetailPostReport(int postId)
        {
            var result = await _adminService.GetPost(postId);
            return Ok(result);
        }

        [Authorize(Policy = "ManageReport")]
        [HttpDelete("delete-post-report/{postId}")]
        public async Task<IActionResult> DeletePostReport(int postId)
        {
            var result = await _adminService.DeletePostReport(postId);
            return Ok(result);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPut("LockAndUnlockAccount")]
        public async Task<ActionResult> LockAndUnlockAC([FromBody] LockAccountDto lockdto)
        {
            await _adminService.SetLock(lockdto);
            return Ok();
        }
        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("GetMembersByAmin")]
        public async Task<ActionResult> GetMembersByAdmin([FromQuery] string username)
        {
            var result = await _adminService.GetUsersByAdmin(username);
            return Ok(result);
        }
    }
}
