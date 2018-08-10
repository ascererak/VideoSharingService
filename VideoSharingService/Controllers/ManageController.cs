using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VideoSharingService.Business;
using VideoSharingService.Data;
using VideoSharingService.Data.Users;
using VideoSharingService.Filters;
using VideoSharingService.Models;

namespace VideoSharingService.Controllers
{
    [Authorize]
    [Culture]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private UserHelper helper = new UserHelper(new Repository());

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        [HttpPost]
        public ActionResult ChangeProfilePhoto()
        {
            return View();
        }

        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? Resources.GlobalRes.PasswordChanged
                : message == ManageMessageId.Error ? Resources.GlobalRes.ErrorOccurred
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // POST: /Manage/ChangePhoto
        [HttpPost]
        public async Task<ActionResult> ChangePhoto(string userId, HttpPostedFileBase upload)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath + Request.UrlReferrer.Query;

            if (ModelState.IsValid)
            {
                string imagePath;

                if (upload != null)
                {
                    string filePath = "/Files/ProfileImages/" + userId + ".jpg";
                    imagePath = filePath;
                    AuthorizedUser user = await UserManager.FindByIdAsync(userId);
                    string oldPhotoPath = Server.MapPath(user.ProfileImagePath);
                    if (user.ProfileImagePath.Contains("ProfileImages") && System.IO.File.Exists(oldPhotoPath))
                    {
                        System.IO.File.Delete(oldPhotoPath);
                    }
                    else
                    {
                        helper.ChangeProfileImage(imagePath, userId);
                    }
                    upload.SaveAs(Server.MapPath("~" + filePath));
                }
                else
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("UserPage", "User", new { userId });
            }
            return Redirect(returnUrl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        //
        // POST: /Manage/DeleteUser
        [HttpPost]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            string currentUserId = User.Identity.GetUserId();
            string returnUrl = Request.UrlReferrer.AbsolutePath + Request.UrlReferrer.Query;
            AuthorizedUser user = await UserManager.FindByIdAsync(userId);
            AuthorizedUser curUser = await UserManager.FindByIdAsync(currentUserId);
            bool isCurUserAdmin = curUser.IsAdmin;
            var userLogins = await UserManager.GetLoginsAsync(userId);

            // Check if current user is not admin
            // user has no rights to delete users 
            // or if current user tries to delete himself
            if (!isCurUserAdmin || userId == currentUserId)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var login in userLogins)
            {
                if (RemoveLogins(userId, login.LoginProvider, login.ProviderKey))
                {
                    return Redirect(returnUrl);
                }
            }

            helper.DeleteUser(userId);

            return Redirect(returnUrl);
        }

        private bool RemoveLogins(string userId, string loginProvider, string providerKey)
        {
            var result = UserManager.RemoveLogin(userId, new UserLoginInfo(loginProvider, providerKey));

            return result.Succeeded;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            var ex = filterContext.Exception;
            var error = new Data.Entities.Error() { ExceptionType = ex.GetType().ToString(), ExceptionText = ex.Message };

            // Save error into DB
            helper.AddError(error);

            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml"
            };
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}