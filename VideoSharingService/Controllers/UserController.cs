using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VideoSharingService.Business;
using VideoSharingService.Data;
using VideoSharingService.Data.Entities;
using VideoSharingService.Data.Users;
using VideoSharingService.Filters;
using VideoSharingService.Models;

namespace VideoSharingService.Controllers
{
    [Culture]
    public class UserController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        UserHelper helper = new UserHelper(new Repository());

        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /User/UserPage
        public async Task<ActionResult> UserPage(string userId, string returnUrl, int? id)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            int page = id ?? 0;
            UserPageViewModel model = new UserPageViewModel()
            {
                User = await UserManager.FindByIdAsync(userId),
                Videos = GetItems(page, userId),
                IsFriend = IsFriend(userId)
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Items", GetItems(page, userId));
            }

            return View(model);
        }

        private List<Video> GetItems(int page, string userId)
        {
            const int pageSize = 2;
            var itemsToSkip = page * pageSize;
            List<Video> videos = helper.GetVideos(userId).ToList();

            return videos.Skip(itemsToSkip).Take(pageSize).ToList();
        }

        //
        // GET: /User/Friends
        public ActionResult Friends()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            var friends = helper.FriendsAsUsers(User.Identity.GetUserId());

            return View(friends);
        }

        //
        // POST: /User/PostVideo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostVideo(UserPageViewModel model)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath + Request.UrlReferrer.Query;
            string userId = User.Identity.GetUserId();

            if (!ModelState.IsValid)
            {
                UserPageViewModel modelOne = new UserPageViewModel()
                {
                    User = await UserManager.FindByIdAsync(userId),
                    Videos = helper.GetVideos(userId).OrderByDescending(s => s.PublishDate),
                    IsFriend = model.IsFriend,
                    Url = model.Url
                };

                return View("UserPage", modelOne);
            }

            string url = model.Url;
            var uri = new Uri(url);

            var query = HttpUtility.ParseQueryString(uri.Query);

            var videoId = string.Empty;

            if (query.AllKeys.Contains("v"))
            {
                videoId = query["v"];
            }
            else
            {
                videoId = uri.Segments.Last();
            }

            url = "https://www.youtube.com/embed/" + videoId;

            Video video = new Video()
            {
                Url = url,
                Author = helper.GetUsers(userId),
                Description = model.Description ?? "",
                PublishDate = DateTime.Now
            };

            helper.AddVideo(video);

            return Redirect(returnUrl);
        }

        //
        // POST: /User/DeleteVideo
        [HttpPost]
        public ActionResult DeleteVideo(int videoId)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath + Request.UrlReferrer.Query;

            helper.DeleteVideo(videoId);

            return Redirect(returnUrl);
        }

        //
        // POST: /User/SubscribeUnsubscribe
        [HttpPost]
        public ActionResult SubscribeUnsubscribe(string userId)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath + Request.UrlReferrer.Query;

            if (!IsFriend(userId))
            {
                helper.AddFriend(userId, User.Identity.GetUserId());
            }
            else
            {
                helper.DeleteFriend(userId, User.Identity.GetUserId());
            }

            return Redirect(returnUrl);
        }

        //
        // POST: /User/Comment
        [HttpPost]
        public ActionResult Comment(VideoCommentViewModel model)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath + Request.UrlReferrer.Query;
            string userId = User.Identity.GetUserId();
            model.Comment.Author = helper.GetUsers(userId);
            model.Comment.Video = helper.GetVideos(model.Video.ID);

            helper.AddComment(model.Comment);

            return Redirect(returnUrl);
        }

        //
        // POST: /User/DeleteComment
        [HttpPost]
        public ActionResult DeleteComment(int commentId)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath + Request.UrlReferrer.Query;

            helper.DeleteComment(commentId);

            return Redirect(returnUrl);
        }

        //
        // POST: /User/EditDescription
        [HttpPost]
        public ActionResult EditDescription(int? videoId, string description)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath + Request.UrlReferrer.Query;

            string newDescription = description ?? "";

            helper.EditDescriprion(videoId, newDescription);

            return Redirect(returnUrl);
        }

        //
        // GET: /User/UserList
        public async Task<ActionResult> UserList()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            string userId = User.Identity.GetUserId();

            AuthorizedUser curUser = await UserManager.FindByIdAsync(userId);

            if (!curUser.IsAdmin)
            {
                return RedirectToAction("Index");
            }

            var users = helper.GetUsers();

            return View(users);
        }

        //
        // GET: /User/PostsList
        public async Task<ActionResult> PostsList()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            string userId = User.Identity.GetUserId();

            AuthorizedUser curUser = await UserManager.FindByIdAsync(userId);

            if (!curUser.IsAdmin)
            {
                return RedirectToAction("Index");
            }
            var videos = helper.GetVideos();

            return View(videos);
        }

        //
        // GET: /User/AdvancedSearch
        public ActionResult AdvancedSearch()
        {
            string userId = User.Identity.GetUserId();
            var user = helper.GetUsers(userId);

            if (!user.IsAdmin)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        //
        // POST: /User/AdvancedSearch
        [HttpPost]
        public ActionResult AdvancedSearch(string searchString)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (string.IsNullOrEmpty(searchString))
            {
                return View();
            }

            AdvancedSearchViewModel model = new AdvancedSearchViewModel();
            ViewBag.SearchString = searchString;

            switch (Request.Form["keyword"].ToString())
            {
                case "users":
                    model.Users = helper.Search(searchString);
                    break;
                case "descriptions":
                    model.Videos = helper.SearchDescription(searchString).ToList();
                    break;
                case "comments":
                    model.Comments = helper.SearchComments(searchString).ToList();
                    break;
                default:
                    break;
            }

            return View(model);
        }

        //
        // GET: /User/SortByFirstName
        public ActionResult SortByFirstName()
        {
            var users = helper.SortByFirstName();

            return View("UserList", users);
        }

        //
        // GET: /User/SortByLastName
        public ActionResult SortByLastName()
        {
            var users = helper.SortByLastName();

            return View("UserList", users);
        }

        //
        // GET: /User/SortByAuthor
        public ActionResult SortByAuthor()
        {
            var videos = helper.SortByAuthor();

            return View("PostsList", videos);
        }

        //
        // GET: /User/SortByDate
        public ActionResult SortByDate()
        {
            var videos = helper.SortByDate();

            return View("PostsList", videos);
        }

        //
        // GET: /User/SortById
        public ActionResult SortById()
        {
            var videos = helper.SortById();

            return View("PostsList", videos);
        }

        //
        // GET: /User/SortByCommentId
        public ActionResult SortByCommentId()
        {
            var comments = helper.SortByCommentId();

            return View("AdvancedSearch", comments);
        }

        //
        // GET: /User/SortByCommentAuthor
        public ActionResult SortByCommentAuthor()
        {
            var comments = helper.SortByCommentAuthor();

            return View("AdvancedSearch", comments);
        }

        private bool IsFriend(string userId)
        {
            List<Friend> friends = (List<Friend>)helper.GetFriends(User.Identity.GetUserId());

            return friends.FirstOrDefault(s => s.UserId == userId && s.Whose == User.Identity.GetUserId()) != null;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            var ex = filterContext.Exception;
            var error = new Error() { ExceptionType = ex.GetType().ToString(), ExceptionText = ex.Message };

            // Save error into DB
            helper.AddError(error);

            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml"
            };
        }
    }
}