using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoSharingService.Data.Entities;
using VideoSharingService.Data.Users;

namespace VideoSharingService.Data
{
    public class VideoSharingContext : IdentityDbContext<AuthorizedUser>
    {
        public VideoSharingContext()
            : base("VideoSharingContext", throwIfV1Schema: false)
        {
        }

        static VideoSharingContext()
        {
            Database.SetInitializer<VideoSharingContext>(new VideoSharingContextInitializer());
        }

        public static VideoSharingContext Create()
        {
            return new VideoSharingContext();
        }

        public DbSet<Video> Videos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Error> Errors { get; set; }
    }
}
