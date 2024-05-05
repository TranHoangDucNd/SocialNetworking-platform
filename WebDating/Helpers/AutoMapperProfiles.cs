using AutoMapper;
using WebDating.DTOs;
using WebDating.DTOs.Post;
using WebDating.Entities.MessageEntities;
using WebDating.Entities.PostEntities;
using WebDating.Entities.ProfileEntities;
using WebDating.Entities.UserEntities;
using WebDating.Extensions;
using WebDating.Utilities;

namespace WebDating.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(d => d.PhotoUrl,
                    opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.CaculateAge()))
                .ForMember(d => d.DatingProfile, 
                    opt => opt.MapFrom(src => src.DatingProfile))
                .ReverseMap();
                

            CreateMap<Photo, PhotoDto>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<MemberUpdateDto, AppUser>();

            CreateMap<Message, MessageDto>() //gửi tin
           .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s =>
                 s.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
           .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s =>
                 s.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ?
                DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);

            CreateMap<UserInterest, UserInterestVM>().ReverseMap();
            CreateMap<DatingProfile, DatingProfileVM>().ReverseMap();

            CreateMap<DatingProfile, DatingProfileDto>()
                .ForMember(dest => dest.WhereToDate,
                    opt => opt.MapFrom(s => s.WhereToDate.GetDisplayName()))
                .ForMember(dest => dest.Height,
                    opt => opt.MapFrom(s => s.Height.GetDisplayName()))
                .ForMember(dest => dest.DatingObject,
                    opt => opt.MapFrom(s => s.DatingObject.GetDisplayName()))
                .ForMember(dest => dest.UserInterests,
                    opt => opt.MapFrom(s => s.UserInterests.Select(ui => new UserInterestDto
                    {
                        Id = ui.Id,
                        DatingProfileId = ui.DatingProfileId,
                        InterestName = ui.InterestName.GetDisplayName()
                    }).ToList()));

            CreateMap<Post, PostResponseDto>()
                .ForMember(dest => dest.Images, o => o.MapFrom(s => s.Images.Select(x => x.Path).ToList()))
                .ForPath(dest => dest.UserShort.Id, o => o.MapFrom(s => s.User.Id))
                .ForPath(dest => dest.UserShort.FullName, o=> o.MapFrom(s=> s.User.UserName))
                .ForPath(dest => dest.UserShort.Image, o=> o.MapFrom(s => s.User.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ReverseMap();

            CreateMap<PostComment, CommentPostDto>()
                .ForPath(dest => dest.UserShort.Id, o => o.MapFrom(s => s.User.Id))
                .ForPath(dest => dest.UserShort.FullName, o=> o.MapFrom(s=> s.User.UserName))
                .ForPath(dest => dest.UserShort.Image, o => o.MapFrom(s => s.User.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ReverseMap();

            CreateMap<PostReportDto, PostReportDetail>().ReverseMap();
          
        }
    }
}
