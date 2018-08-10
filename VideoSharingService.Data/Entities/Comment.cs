using System;
using System.ComponentModel.DataAnnotations;
using VideoSharingService.Data.Users;

namespace VideoSharingService.Data
{
    public class Comment
    {
        [Key]
        public int ID { get; set; }
        public string CommentText { get; set; }
        public virtual AuthorizedUser Author { get; set; }
        public virtual Video Video { get; set; }
    }
}
