using VideoSharingService.Data;

namespace VideoSharingService.Models
{
    public class VideoCommentViewModel
    {
        public Video Video { get; set; }
        public Comment Comment { get; set; }
    }
}