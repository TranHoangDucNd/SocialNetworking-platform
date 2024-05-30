using Microsoft.AspNetCore.Identity;
using WebDating.Entities.MessageEntities;
using WebDating.Entities.PostEntities;
using WebDating.Entities.ProfileEntities;
using System.ComponentModel.DataAnnotations.Schema;
using WebDating.Entities.NotificationEntities;
namespace WebDating.Entities.UserEntities
{
    public class AppUser : IdentityUser<int>
    {
        public byte[] PasswordSalt { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public bool IsUpdatedDatingProfile { get; set; } = false;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string City { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }

        public bool Lock { get; set; } = false;
        public DatingProfile DatingProfile { get; set; }
        public List<Photo> Photos { get; set; } = new();

        //Follow
        public List<UserLike> LikedByUsers { get; set; }
        public List<UserLike> LikedUsers { get; set; }

        //Message
        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesReceived { get; set; }

        //Role
        public ICollection<AppUserRole> UserRoles { get; set; }

        //Post -------
        public ICollection<Post> Posts { get; set; }
        public ICollection<PostReportDetail> PostReportDetails { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

        //Dating request
        public virtual ICollection<DatingRequest> DatingRequests { get; set; }
    }
}
