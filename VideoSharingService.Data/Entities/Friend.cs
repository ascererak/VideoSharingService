using System;
using System.ComponentModel.DataAnnotations;

namespace VideoSharingService.Data.Entities
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual string Whose { get; set; }
    }
}
