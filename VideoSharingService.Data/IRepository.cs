using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoSharingService.Data.Entities;
using VideoSharingService.Data.Users;

namespace VideoSharingService.Data
{
    public interface IRepository
    {
        IEnumerable<Video> Videos { get; }
        IEnumerable<Comment> Comments { get; }
        IEnumerable<AuthorizedUser> Users { get; }
        IEnumerable<Friend> Friends { get; }
        IEnumerable<Error> Errors { get; }
        void AddVideo(Video video);
        void DeleteVideo(int videoId);
        void AddFriend(string userId, string curUserId);
        void DeleteFriend(string userId, string curUserId);
        void AddComment(Comment comment);
        void DeleteComment(int commentId);
        void EditDescription(int? videoId, string newDescription);
        void DeleteUser(string userId);
        void ChangeProfileImage(string imagePath, string userId);
        void AddError(Error error);
        void DeleteAllComments(IEnumerable<Comment> comments);
    }
}
