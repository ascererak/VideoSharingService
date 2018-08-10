using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VideoSharingService.Data.Entities;

namespace VideoSharingService.Data.Users
{
    public class AuthorizedUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AuthorizedUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }

        public bool IsAdmin { get; set; }
        public string ProfileImagePath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
