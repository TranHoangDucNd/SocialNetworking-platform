using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using WebDating.DTOs;
using WebDating.DTOs.Post;
using WebDating.Entities;
using WebDating.Interfaces;

namespace WebDating.Services
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPhotoService _photoService;

        public PostService(IMapper mapper, IUnitOfWork uow, UserManager<AppUser> userManager,
            IPhotoService photoService)
        {
            _mapper = mapper;
            _uow = uow;
            _userManager = userManager;
            _photoService = photoService;
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
            return new SuccessResult<UserShortDto>(new UserShortDto() { 
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
    }
}
