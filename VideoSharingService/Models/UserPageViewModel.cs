using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VideoSharingService.Data;
using VideoSharingService.Data.Users;

namespace VideoSharingService.Models
{
    public class UserPageViewModel
    {
        public AuthorizedUser User { get; set; }
        public IEnumerable<Video> Videos { get; set; }
        public bool IsFriend { get; set; }
        [Required]
        [YouTubeUrl]
        public string Url { get; set; }
        public string Description { get; set; }
    }
}