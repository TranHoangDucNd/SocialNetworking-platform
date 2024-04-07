using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebDating.DTOs;
using WebDating.Entities;
using WebDating.Interfaces;

namespace WebDating.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _uow.UserRepository.GetMembersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetUserById(int id)
        {
            return await _uow.UserRepository.GetUserByIdAsync(id);
        }

        [HttpGet("member/{username}")]
        public async Task<ActionResult<MemberDto>> GetMember(string username)
        {
            return await _uow.UserRepository.GetMemberAsync(username);
        }

    }
}
