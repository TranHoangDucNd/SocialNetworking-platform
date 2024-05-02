using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebDating.DTOs;
using WebDating.Entities.ProfileEntities;
using WebDating.Interfaces;

namespace WebDating.Services
{
    public class DatingService : IDatingService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public DatingService(IMapper mapper, IUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        //lưu vào db
        public async Task<ResultDto<DatingProfileVM>> InitDatingProfile(DatingProfileVM datingProfileVM, string userName)
        {
            try
            {
                var dating = _mapper.Map<DatingProfile>(datingProfileVM);
                var user = await _uow.UserRepository.GetUserByUsernameAsync(userName);
                dating.UserId = user.Id;
                //Lưu sở thích của userId vào bảng datingprofile
                var result = await _uow.DatingRepository.Insert(dating);
                await _uow.Complete();
                //Lưu hết r set user update = true

                user.IsUpdatedDatingProfile = true;

                _uow.UserRepository.UpdateUser(user);
                _uow.CompleteNotAsync();

                await _uow.DatingRepository.InsertUserInterest(dating.UserInterests, dating.Id);

                return new SuccessResult<DatingProfileVM>(result);
            }
            catch (Exception ex)
            {
                return new ErrorResult<DatingProfileVM>(ex.Message);
            }

        }
    }
}
