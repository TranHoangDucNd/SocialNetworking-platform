using AutoMapper;
using WebDating.Data;
using WebDating.DTOs;
using WebDating.DTOs.Post;
using WebDating.Helpers;
using WebDating.Interfaces;

namespace WebDating.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public AdminService(IUnitOfWork uow, IMapper mapper, IPhotoService photoService, DataContext dataContext)
        {
            _uow = uow;
            _mapper = mapper;
            _photoService = photoService;
    
        }
        public async Task<ResultDto<List<PostReportAdminDto>>> DeletePostReport(int postId)
        {
            var post = await _uow.PostRepository.GetById(postId);
            _uow.AdminRepository.Delete(post);
            await _uow.Complete();
            return await GetPostReports();
        }

        public async Task<ResultDto<ShowPostAdminDto>> GetPost(int postId)
        {
            var result = await _uow.AdminRepository.GetPostByID(postId);
            return new SuccessResult<ShowPostAdminDto>(_mapper.Map<ShowPostAdminDto>(result));
        }

        public async Task<ResultDto<List<PostReportAdminDto>>> GetPostReports()
        {
            var result = await _uow.AdminRepository.GetPostReports();
            return new SuccessResult<List<PostReportAdminDto>>(_mapper.Map<List<PostReportAdminDto>>(result));
        }

       
    }
}
