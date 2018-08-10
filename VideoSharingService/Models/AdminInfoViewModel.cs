using System.Collections.Generic;
using VideoSharingService.Business;
using VideoSharingService.Data;
using VideoSharingService.Data.Users;

namespace VideoSharingService.Models
{
    public class AdminInfoViewModel
    {
        public IEnumerable<AuthorizedUser> Users { get; set; }
        public IEnumerable<Video> Videos { get; set; }

        public bool IsAdmin(string userId)
        {
            var client = new UserHelper(new Repository());
            bool isAdmin = client.GetUsers(userId).IsAdmin;

            return isAdmin;
        }
    }
}