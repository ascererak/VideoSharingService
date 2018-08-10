using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VideoSharingService.Business;
using VideoSharingService.Data;
using VideoSharingService.Data.Entities;
using VideoSharingService.Filters;

namespace VideoSharingService.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        UserHelper helper = new UserHelper(new Repository());

        //
        // GET: /Home/Index
        public ActionResult Index(int? id)
        {
            if (!Request.IsAuthenticated)
            {
                return View();
            }

            int page = id ?? 0;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Items", GetItems(page));
            }

            return View(GetItems(page));
        }

        private List<Video> GetItems(int page)
        {
            const int pageSize = 2;
            var itemsToSkip = page * pageSize;
            string userId = User.Identity.GetUserId();
            List<Video> feed = (List<Video>)helper.GetFeed(userId);

            return feed.Skip(itemsToSkip).Take(pageSize).ToList();
        }

        //
        // GET: /Home/ChangeCulture
        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath + Request.UrlReferrer.Query;

            List<string> cultures = new List<string>() { "en", "ru" };

            if (!cultures.Contains(lang))
            {
                lang = "en";
            }

            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
            {
                cookie.Value = lang;
            }
            else
            {
                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);

            return Redirect(returnUrl);
        }

        //
        // GET: /Home/Search
        public ActionResult Search(string searchString)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath + Request.UrlReferrer.Query;
            
            ViewBag.SearchString = searchString;

            if (!ModelState.IsValid || string.IsNullOrEmpty(searchString))
            {
                return Redirect(returnUrl);
            }

            var users = helper.Search(searchString);
            Dictionary<string, bool> areFriends = new Dictionary<string, bool>();

            try
            {
                foreach (var user in users)
                {
                    areFriends.Add(user.Id, IsFriend(user.Id));
                }
            }
            catch(NullReferenceException)
            {
                Redirect(returnUrl);
            }

            ViewBag.AreFriends = areFriends;

            return View(users);
        }

        private bool IsFriend(string userId)
        {
            string currentUserId = User.Identity.GetUserId();
            List<Friend> friends = (List<Friend>)helper.GetFriends(currentUserId);

            return friends.FirstOrDefault(s => s.UserId == userId && s.Whose == currentUserId) != null;
        }

        //
        // GET: /Home/AboutSystem
        public ActionResult AboutSystem()
        {
            return View();
        }

        //
        // GET: /Home/TermsOfService
        public ActionResult TermsOfService()
        {
            return View();
        }

        //
        // GET: /Home/Details
        public async Task<ActionResult> Details(int videoId)
        {
            Video video = await helper.GetVideosAsync(videoId);

            if (video != null)
            {
                return PartialView(video);
            }

            return HttpNotFound();
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

        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}