using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebDating.DTOs;
using WebDating.Entities.ProfileEntities;
using WebDating.Interfaces;
using WebDating.Utilities;

namespace WebDating.Controllers
{
    public class DatingProfileController : BaseController
    {
        private readonly IDatingService _service;

        public DatingProfileController(IDatingService service)
        {
            _service = service;
        }

        [HttpGet("GetDatingObject")]
        public ActionResult<IEnumerable<EnumT<Gender>>> GetDatingObject()
        {
            var result = Utils.GetAllAccountType<Gender>();
            return Ok(result);
        }

        [HttpGet("GetHeight")]
        public ActionResult<IEnumerable<EnumT<Height>>> GetHeight()
        {
            var result = Utils.GetAllAccountType<Height>();
            return Ok(result);
        }

        [HttpGet("GetWhereToDate")]
        public ActionResult<IEnumerable<EnumT<Provice>>> GetWhereToDate()
        {
            var result = Utils.GetAllAccountType<Provice>();
            return Ok(result);
        }


        [HttpGet("GetUserInterests")]
        public ActionResult<IEnumerable<EnumT<Interest>>> GetUserInterests()
        {
            var result = Utils.GetAllAccountType<Interest>();
            return Ok(result);
        }
        [HttpGet("GetUserOccupations")]
        public ActionResult<IEnumerable<EnumT<Occupation>>> GetUserOccupations()
        {
            var result = Utils.GetAllAccountType<Occupation>();
            return Ok(result);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResultDto<DatingProfileVM>>> CreateDatingProfile([FromForm] DatingProfileVM datingProfile)
        {
            IReadOnlyList<UserInterestVM> UserInterestVM;
            {
                String formList = this.Request.Form["UserInterests"];
                UserInterestVM = JsonConvert.DeserializeObject<List<UserInterestVM>>(formList) ?? new();
                datingProfile.UserInterests = UserInterestVM.ToList();
            }
            var result = await _service.InitDatingProfile(datingProfile, User.Identity.Name);
            return Ok(result);
        }
    }
}
