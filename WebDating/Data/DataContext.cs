
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebDating.Entities.MessageEntities;
using WebDating.Entities.NotificationEntities;
using WebDating.Entities.PostEntities;
using WebDating.Entities.ProfileEntities;
using WebDating.Entities.UserEntities;

namespace WebDating.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole,
        IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<UserLike> Likes { get; set; }

        //Message Signalr
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }

        //Profile
        public DbSet<DatingProfile> DatingProfiles { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }

        //Post
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostsComments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<PostReportDetail> PostReportDetails { get; set; }
        public DbSet<PostSubComment> PostSubComments { get; set; }
        public DbSet<ImagePost> ImagePosts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<ReactionLog> ReactionLogs { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<DatingRequest> DatingRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>()
                .HasKey(k => new { k.TargetUserId, k.SourceUserId });

            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>()
                .HasOne(t => t.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(t => t.TargetUserId)
                .OnDelete(DeleteBehavior.ClientCascade);


            builder.Entity<Message>()
            .HasOne(u => u.Recipient)
            .WithMany(m => m.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);//vd khi người dùng xóa tài khoản thì tin nhắn họ gửi phía người nhận vẫn còn

            builder.Entity<Message>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<UserInterest>()
                .HasOne(x => x.DatingProfile)
                .WithMany(x => x.UserInterests)
                .HasForeignKey(x => x.DatingProfileId);

            builder.Entity<AppUser>()
                .HasOne<DatingProfile>()
                .WithOne(x => x.User)
                .HasForeignKey<DatingProfile>(x => x.UserId);


            //Post


            #region Post
            builder.Entity<Post>(entity =>
            {
                entity.HasOne(d => d.User)
                .WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Post__UserId__778AC167");
            });

            //builder.Entity<PostComment>(entity =>
            //{
            //    entity.HasOne(x => x.Post)
            //    .WithMany(x => x.PostComments)
            //    .HasForeignKey(x => x.PostId)
            //    .HasConstraintName("FK__PostComme__PostI__0F624AF8");

            //    entity.HasOne(x => x.User)
            //    .WithMany(x => x.PostComments)
            //    .HasForeignKey(x => x.UserId)
            //    .HasConstraintName("FK__PostComme__UserI__10566F31");

            //});

            builder.Entity<PostLike>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__PostLike__PostId__1332DBDC");
                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__PostLike__UserId__14270015");
            });

            builder.Entity<ImagePost>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__PostImage__PostId__1332DBDC");
            });


            builder.Entity<PostReportDetail>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostReportDetails)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__PostRepor__PostI__17036CC0");


                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostReportDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__PostRepor__UserI__160F4887");
            });

            builder.Entity<PostSubComment>(entity =>
            {
                entity.HasOne(d => d.PreComment)
                    .WithMany(p => p.PostSubComments)
                    .HasForeignKey(d => d.PreCommentId)
                    .HasConstraintName("FK__PostSubCo__PreCo__114A936A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostSubComments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__PostSubCo__UserI__123EB7A3");
            });
            #endregion


            #region Post Comment
            builder.Entity<Comment>(e =>
            {
                e.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(k => k.PostId);
            });

            builder.Entity<ReactionLog>(e =>
            {
                e.HasOne(c => c.Comment)
                .WithMany(r => r.ReactionLogs)
                .HasForeignKey(k => k.CommentId);
            });
            #endregion

            #region Reaction
            builder.Entity<ReactionLog>(e =>
            {
                e.HasOne(c => c.Post)
                .WithMany(r => r.ReactionLogs)
                .HasForeignKey(k => k.PostId);
            });

            builder.Entity<ReactionLog>()
                .ToTable(t => t.HasCheckConstraint("CheckForeignKeyCount", "(CommentId IS NOT NULL AND PostId IS NULL) OR (CommentId IS NULL AND PostId IS NOT NULL)"));
            #endregion

            #region Notification
            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(f => f.NotifyToUserId);
            builder.Entity<Notification>()
               .HasOne(n => n.Post)
               .WithMany(u => u.Notifications)
               .HasForeignKey(f => f.PostId);
            builder.Entity<Notification>()
              .HasOne(n => n.Comment)
              .WithMany(u => u.Notifications)
              .HasForeignKey(f => f.CommentId);

            builder.Entity<Notification>()
              .HasOne<DatingRequest>()
              .WithMany(x => x.Notifications)
              .HasForeignKey(x => x.DatingRequestId);

            //builder.Entity<ReactionLog>()
            //    .ToTable(t => t.HasCheckConstraint("CheckForeignKeyCount", "(CommentId IS NOT NULL AND PostId IS NULL) OR (CommentId IS NULL AND PostId IS NOT NULL)"));
            #endregion


            #region Dating Request
            builder.Entity<DatingRequest>()
                .HasOne<AppUser>()
                .WithMany(x => x.DatingRequests)
                .HasForeignKey(x => x.SenderId);

            builder.Entity<DatingRequest>()
              .HasOne<AppUser>()
              .WithMany(x => x.DatingRequests)
              .HasForeignKey(x => x.CrushId);

         

            #endregion
        }
    }
}
