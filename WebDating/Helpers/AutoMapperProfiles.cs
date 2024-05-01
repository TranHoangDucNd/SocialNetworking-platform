using AutoMapper;
using WebDating.DTOs;
using WebDating.DTOs.Post;
using WebDating.Entities;
using WebDating.Extensions;

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
                    opt => opt.MapFrom(src => src.DateOfBirth.CaculateAge()));

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

            CreateMap<Post, PostResponseDto>()
                .ForMember(dest => dest.Images, o => o.MapFrom(s => s.Images.Select(x => x.Path).ToList()))
                .ForPath(dest => dest.UserShort.Id, o => o.MapFrom(s => s.User.Id))
                .ForPath(dest => dest.UserShort.FullName, o=> o.MapFrom(s=> s.User.UserName))
                .ForPath(dest => dest.UserShort.Image, o=> o.MapFrom(s => s.User.Photos.Select(s => s.Url).FirstOrDefault()))
                .ReverseMap();

            CreateMap<PostComment, CommentPostDto>().ReverseMap();

            
          
        }
    }
}
