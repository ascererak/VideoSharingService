using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VideoSharingService.Data.Users;

namespace VideoSharingService.Data
{
    public class Video
    {
        [Key]
        public int ID { get; set; }
        public string Url { get; set; }        
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public virtual AuthorizedUser Author { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
