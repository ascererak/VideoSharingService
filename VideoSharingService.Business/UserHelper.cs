using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using VideoSharingService.Data;
using VideoSharingService.Data.Entities;
using VideoSharingService.Data.Users;

namespace VideoSharingService.Business
{
    public class UserHelper
    {
        private IRepository repository;

        public UserHelper(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Get all the users
        /// </summary>
        /// <returns>Collection of users</returns>
        public IEnumerable<AuthorizedUser> GetUsers()
        {
            return repository.Users.ToList();
        }

        /// <summary>
        /// Get a specific user by id
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>Specific user</returns>
        public AuthorizedUser GetUsers(string id)
        {
            return repository.Users.FirstOrDefault(s => s.Id == id);
        }

        /// <summary>
        /// Get all the videos
        /// </summary>
        /// <returns>Colleciot on videos</returns>
        public IEnumerable<Video> GetVideos()
        {
            return repository.Videos.ToList();
        }

        /// <summary>
        /// Get a specific video asynchronous
        /// </summary>
        /// <param name="id">Video id</param>
        /// <returns>Specific video</returns>
        public Video GetVideos(int id)
        {
            var videos = repository.Videos;
            return videos.FirstOrDefault(s => s.ID == id);
        }

        /// <summary>
        /// Get a specific video asynchronous
        /// </summary>
        /// <param name="id">Video id</param>
        /// <returns>Specific video</returns>
        public async Task<Video> GetVideosAsync(int id)
        {
            var videos = (IQueryable<Video>)repository.Videos;
            return await videos.FirstOrDefaultAsync(s => s.ID == id);
        }

        /// <summary>
        /// Get all the videos by specific user
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <returns>Collection of videos by specific user</returns>
        public IEnumerable<Video> GetVideos(string userId)
        {
            AuthorizedUser user = GetUsers(userId);
            var videos = GetVideos().Where(s => s.Author == user).ToList();

            return videos.OrderByDescending(s => s.PublishDate).ToList();
        }

        /// <summary>
        /// Get all the friends of a specific user
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <returns>Collection of friends of a specific user</returns>
        public IEnumerable<Friend> GetFriends(string userId)
        {
            return repository.Friends.Where(s => s.Whose == userId).ToList();
        }

        /// <summary>
        /// Get friends as users
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <returns>Collection of users</returns>
        public IEnumerable<AuthorizedUser> FriendsAsUsers(string userId)
        {
            var friendsUsers = new List<AuthorizedUser>();
            var friends = GetFriends(userId);

            foreach (var friend in friends)
            {
                friendsUsers.Add(GetUsers(friend.UserId));
            }

            return friendsUsers;
        }

        /// <summary>
        /// Get all user's friends vidios
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <returns>Collection of vidios</returns>
        public IEnumerable<Video> GetFeed(string userId)
        {
            List<Video> feed = new List<Video>();
            List<Friend> friends = GetFriends(userId).ToList();

            feed.AddRange(GetVideos(userId));

            foreach (Friend friend in friends)
            {
                feed.AddRange(GetVideos(friend.UserId));
            }

            feed.OrderByDescending(s => s.PublishDate);

            return feed;
        }

        /// <summary>
        /// Search for users
        /// </summary>
        /// <param name="searchString">Search request</param>
        /// <returns>Collection of found users</returns>
        public IEnumerable<AuthorizedUser> Search(string searchString)
        {
            var users = GetUsers();

            return users.Where(s => (s.FirstName + s.LastName).ToLower().Contains(searchString.ToLower()));
        }

        /// <summary>
        /// Search for videos, where description contains specific string
        /// </summary>
        /// <param name="searchString">Search request</param>
        /// <returns>Collection of found videos</returns>
        public IEnumerable<Video> SearchDescription(string searchString)
        {
            var videos = GetVideos();

            return videos.Where(s => s.Description.ToLower().Contains(searchString.ToLower()));
        }

        /// <summary>
        /// Search for comments, that contain a specific string
        /// </summary>
        /// <param name="searchString">Search request</param>
        /// <returns>Collection of found comments</returns>
        public IEnumerable<Comment> SearchComments(string searchString)
        {
            var comments = repository.Comments;

            return comments.Where(s => s.CommentText.ToLower().Contains(searchString.ToLower()));
        }

        public void AddVideo(Video video)
        {
            repository.AddVideo(video);
        }

        public void DeleteVideo(int videoId)
        {
            var videos = GetVideos();
            var video = videos.FirstOrDefault(s => s.ID == videoId);

            if (video.Comments.Count > 0)
            {
                DeleteAllComments(video.Comments);
            }

            repository.DeleteVideo(videoId);
        }

        private void DeleteAllComments(IEnumerable<Comment> comments)
        {
            repository.DeleteAllComments(comments);
        }

        public void AddFriend(string userId, string curUserId)
        {
            repository.AddFriend(userId, curUserId);
        }

        public void DeleteFriend(string userId, string curUserId)
        {
            repository.DeleteFriend(userId, curUserId);
        }

        public void AddComment(Comment comment)
        {
            repository.AddComment(comment);
        }

        public void DeleteComment(int commentId)
        {
            repository.DeleteComment(commentId);
        }

        public void EditDescriprion(int? videoId, string newDescription)
        {
            repository.EditDescription(videoId, newDescription);
        }

        public void DeleteUser(string userId)
        {
            var videos = GetVideos(userId);

            foreach (var video in videos)
            {
                DeleteVideo(video.ID);
            }

            repository.DeleteUser(userId);
        }

        public void AddError(Error error)
        {
            repository.AddError(error);
        }

        public IEnumerable<AuthorizedUser> SortByFirstName()
        {
            var users = GetUsers();

            return users.OrderBy(s => s.FirstName);
        }

        public IEnumerable<AuthorizedUser> SortByLastName()
        {
            var users = GetUsers();

            return users.OrderBy(s => s.LastName);
        }

        public IEnumerable<Video> SortByDate()
        {
            var videos = GetVideos();

            return videos.OrderBy(s => s.PublishDate);
        }

        public IEnumerable<Video> SortByAuthor()
        {
            var videos = GetVideos();

            return videos.OrderBy(s => s.Author.FirstName);
        }

        public IEnumerable<Video> SortById()
        {
            var videos = GetVideos();

            return videos.OrderBy(s => s.ID);
        }

        public IEnumerable<Comment> SortByCommentId()
        {
            var comments = repository.Comments;

            return comments.OrderBy(s => s.ID);
        }

        public IEnumerable<Comment> SortByCommentAuthor()
        {
            var comments = repository.Comments;

            return comments.OrderBy(s => s.Author.FirstName);
        }

        public void ChangeProfileImage(string imagePath, string userId)
        {
            repository.ChangeProfileImage(imagePath, userId);
        }
    }
}
