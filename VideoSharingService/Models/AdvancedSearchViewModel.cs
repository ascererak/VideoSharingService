using System.Collections.Generic;
using VideoSharingService.Data;
using VideoSharingService.Data.Users;

namespace VideoSharingService.Models
{
    public class AdvancedSearchViewModel
    {
        public IEnumerable<AuthorizedUser> Users { get; set; }
        public IEnumerable<Video> Videos { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}