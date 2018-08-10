using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoSharingService.Data;
using VideoSharingService.Data.Users;

namespace VideoSharingService
{
    class VideoSharingContextInitializer : DropCreateDatabaseIfModelChanges<VideoSharingContext>
    {
        protected override void Seed(VideoSharingContext context)
        {
            UserManager<AuthorizedUser> userManager = new UserManager<AuthorizedUser>(new UserStore<AuthorizedUser>(context));

            List<AuthorizedUser> users = new List<AuthorizedUser>
            {
                new AuthorizedUser
                {
                    FirstName = "Alexey",
                    LastName = "Bondar",
                    Email = "lexhedgehog@gmail.com",
                    UserName = "lexhedgehog@gmail.com",
                    ProfileImagePath = "/Files/DefaultImage.jpg",
                    EmailConfirmed = true,
                    IsAdmin = true
                },
                new AuthorizedUser
                {
                    FirstName = "Jon",
                    LastName = "Snow",
                    Email = "exampleemail@gmail.com",
                    UserName = "exampleemail@gmail.com",
                    ProfileImagePath = "/Files/DefaultImage.jpg",
                    EmailConfirmed = true
                }
            };

            string password = "Qwerty123";

            foreach (var user in users)
            {
                userManager.Create(user, password);
            }

            List<List<Video>> allTheVideos = new List<List<Video>>
            {
                new List<Video>
                {
                    new Video
                    {
                        Url = "https://www.youtube.com/embed/SAQLDbFGypU",
                        Description = "Video from Casey",
                        PublishDate = DateTime.Now,
                        Author = users[0]
                    },
                    new Video
                    {
                        Url = "https://www.youtube.com/embed/UgnbIJYUTQY",
                        Description = "MVP pattern",
                        PublishDate = DateTime.Now,
                        Author = users[0]
                    },
                },
                new List<Video>
                {
                    new Video
                    {
                        Url = "https://www.youtube.com/embed/6qYIiD49OVs",
                        Description = "Видео про Git",
                        PublishDate = DateTime.Now,
                        Author = users[1]
                    },
                    new Video
                    {
                        Url = "https://www.youtube.com/embed/aIkpVzqLuhA",
                        Description = "Asp.Net Core 2.0",
                        PublishDate = DateTime.Now,
                        Author = users[1]
                    },
                    new Video
                    {
                        Url = "https://www.youtube.com/embed/hMAPyGoqQVw",
                        Description = "",
                        PublishDate = DateTime.Now,
                        Author = users[1]
                    }
                },
            };

            foreach (var videos in allTheVideos)
            {
                context.Videos.AddRange(videos);
                context.SaveChanges();
            }
        }
    }
}
