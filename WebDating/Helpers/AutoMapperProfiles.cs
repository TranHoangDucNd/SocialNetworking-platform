using AutoMapper;
using WebDating.DTOs;
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
            

            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
        }
    }
}
