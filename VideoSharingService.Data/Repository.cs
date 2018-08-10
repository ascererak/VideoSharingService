using System.Collections.Generic;
using System.Linq;
using VideoSharingService.Data.Users;
using VideoSharingService.Data.Entities;

namespace VideoSharingService.Data
{
    public class Repository : IRepository
    {
        private readonly VideoSharingContext context = new VideoSharingContext();

        public IEnumerable<Video> Videos => context.Videos;

        public IEnumerable<Comment> Comments => context.Comments;

        public IEnumerable<AuthorizedUser> Users => context.Users;

        public IEnumerable<Friend> Friends => context.Friends;

        public IEnumerable<Error> Errors => context.Errors;

        public void AddVideo(Video video)
        {
            context.Videos.Add(video);

            context.SaveChanges();
        }

        public void DeleteVideo(int videoId)
        {
            context.Videos.Remove(Videos.First(s => s.ID == videoId));

            context.SaveChanges();
        }

        public void AddFriend(string userId, string curUserId)
        {
            Friend friend = new Friend() { UserId = userId, Whose = curUserId };

            context.Friends.Add(friend);

            context.SaveChanges();
        }

        public void DeleteFriend(string userId, string curUserId)
        {
            Friend friend = Friends.FirstOrDefault(s => s.UserId == userId && s.Whose == curUserId);

            context.Friends.Remove(friend);

            context.SaveChanges();
        }

        public void AddComment(Comment comment)
        {
            context.Comments.Add(comment);

            context.SaveChanges();
        }

        public void DeleteComment(int commentId)
        {
            Comment comment = Comments.FirstOrDefault(s => s.ID == commentId);

            context.Comments.Remove(comment);

            context.SaveChanges();
        }

        public void EditDescription(int? videoId, string newDescription)
        {
            Video video = context.Videos.FirstOrDefault(s => s.ID == videoId);

            video.Description = newDescription;

            context.SaveChanges();
        }

        public void DeleteUser(string userId)
        {
            var user = context.Users.FirstOrDefault(s => s.Id == userId);

            context.Users.Remove(user);

            context.SaveChanges();
        }

        public void ChangeProfileImage(string imagePath, string userId)
        {
            var user = context.Users.FirstOrDefault(s => s.Id == userId);

            user.ProfileImagePath = imagePath;

            context.SaveChanges();
        }

        public void AddError(Error error)
        {
            context.Errors.Add(error);

            context.SaveChanges();
        }

        public void DeleteAllComments(IEnumerable<Comment> comments)
        {
            context.Comments.RemoveRange(comments);

            context.SaveChanges();
        }
    }
}
