using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebDating.DTOs;
using WebDating.Entities;
using WebDating.Extensions;
using WebDating.Helpers;
using WebDating.Interfaces;

namespace WebDating.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UsersController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            userParams.CurrentUserName = User.GetUserName();
            var gender = await _uow.UserRepository.GetUserGender(User.GetUserName());

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = gender == "female" ? "male" : "female";
            }

            var users = await _uow.UserRepository.GetMembersAsync(userParams);

            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetMember(string username)
        {
            return await _uow.UserRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.GetUserName();
            var user = await _uow.UserRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user);

            if (await _uow.Complete()) return NoContent();

            return BadRequest("Failed update");
        }



    }
}
