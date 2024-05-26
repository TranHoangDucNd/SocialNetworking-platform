using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebDating.Data;
using WebDating.DTOs;
using WebDating.Entities;
using WebDating.Entities.MessageEntities;
using WebDating.Extensions;
using WebDating.Helpers;
using WebDating.Interfaces;
using WebDating.Services;

namespace WebDating.Controllers
{
    [Authorize]
    public class MessagesController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public MessagesController(IUnitOfWork uow, IMapper mapper, IPhotoService photoService)
        {
            _uow = uow;
            _mapper = mapper;
            _photoService = photoService;
        }

        [Authorize]
        [HttpPost]
        [Route("upload-image-message")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            var uploadResult = await _photoService.AddPhotoAsync(file);
            var path = uploadResult.SecureUrl.AbsoluteUri;
            var publicId = uploadResult.PublicId;

            return Ok(new { Path = path, PublicId = publicId });
        }


        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {

            var username = User.GetUserName();

            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot sent message to yourself!");

            var sender = await _uow.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _uow.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content,
            };

            _uow.MessageRepository.AddMessage(message);

            if (await _uow.Complete()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to sent message!");

        }


        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUserName();

            var messages = await _uow.MessageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize,
            messages.TotalCount, messages.TotalPages));

            return messages;

        }

        [HttpGet]
        [Route("get-messages-for-user")]
        public ActionResult<List<MessageDto>> GetMessages()
        {
            var username = User.GetUserName();
            var result = _uow.MessageRepository.GetMessages(username);
            return Ok(result);
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesThread(string username)
        {
            var currentUserName = User.GetUserName();

            return Ok(await _uow.MessageRepository.GetMessageThread(currentUserName, username));
        }

        [HttpDelete]
        [Route("delete-messages-by-username/{otherUsername}")]
        public async Task<IActionResult> DeleteAllMessagesByUserId(string otherUsername)
        {
            string currentUsername = User.GetUserName();
             await _uow.MessageRepository.DeleteAllMessageByUserId(currentUsername, otherUsername);
            if(await _uow.Complete()) return NoContent();
            return BadRequest("Delete message failed");
            
        }
    }
}
