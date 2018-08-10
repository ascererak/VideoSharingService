using System;
using System.ComponentModel.DataAnnotations;

namespace VideoSharingService.Data.Entities
{
    public class Error
    {
        [Key]
        public int Id { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionText { get; set; }
    }
}
