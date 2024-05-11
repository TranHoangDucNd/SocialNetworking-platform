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
                .ForMember(dest => dest.WhereToDateCode,
                    opt => opt.MapFrom(s => s.WhereToDate))
                .ForMember(dest => dest.Height,
                    opt => opt.MapFrom(s => s.Height.GetDisplayName()))
                .ForMember(dest => dest.HeightCode,
                    opt => opt.MapFrom(s => s.Height))
                .ForMember(dest => dest.DatingObject,
                    opt => opt.MapFrom(s => s.DatingObject.GetDisplayName()))
                .ForMember(dest => dest.DatingObjectCode,
                    opt => opt.MapFrom(s => s.DatingObject))
                .ForMember(dest => dest.UserInterests,
                    opt => opt.MapFrom(s => s.UserInterests.Select(ui => new UserInterestDto
                    {
                        Id = ui.Id,
                        DatingProfileId = ui.DatingProfileId,
                        InterestName = ui.InterestName.GetDisplayName(),
                        InterestNameCode = ui.InterestName
                    }).ToList()));

            CreateMap<Post, PostResponseDto>()
                .ForMember(dest => dest.LikeNumber, o => o.MapFrom(m => m.ReactionLogs.Count()))
                .ForMember(dest => dest.CommentNumber, o => o.MapFrom(m => m.Comments.Count()))
                .ForMember(dest => dest.Images, o => o.MapFrom(s => s.Images.Select(x => x.Path).ToList()))
                .ForPath(dest => dest.UserShort.Id, o => o.MapFrom(s => s.User.Id))
                .ForPath(dest => dest.UserShort.FullName, o => o.MapFrom(s => s.User.UserName))
                .ForPath(dest => dest.UserShort.Image, o => o.MapFrom(s => s.User.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ReverseMap();

            CreateMap<Post, ShowPostAdminDto>()
                .ForMember(dest => dest.Images, o => o.MapFrom(s => s.Images.Select(x => x.Path).ToList()))
                .ForPath(dest => dest.UserShort.Id, o => o.MapFrom(s => s.UserId))
                .ForPath(dest => dest.UserShort.FullName, o => o.MapFrom(s => s.User.UserName))
                .ForPath(dest => dest.UserShort.Image, o => o.MapFrom(s => s.User.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<PostComment, CommentPostDto>()
                .ForPath(dest => dest.UserShort.Id, o => o.MapFrom(s => s.User.Id))
                .ForPath(dest => dest.UserShort.FullName, o => o.MapFrom(s => s.User.UserName))
                .ForPath(dest => dest.UserShort.Image, o => o.MapFrom(s => s.User.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ReverseMap();

            CreateMap<PostReportDto, PostReportDetail>()
                .ReverseMap();

            CreateMap<PostReportDetail, PostReportAdminDto>()
              .ForMember(dest => dest.Report, opt => opt.MapFrom(x => x.Report.GetDisplayName()));

            CreateMap<AppUser, MembersLockDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url));

        }
    }
}
