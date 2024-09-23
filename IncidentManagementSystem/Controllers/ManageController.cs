using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using IncidentManagementSystem.Models;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System.Collections.Generic;
using IncidentManagementSystem.Model;
using IncidentManagementSystem.Service;
using System.Web.Security;
using System.Security.Claims;

namespace IncidentManagementSystem.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IInstitutionService _iInstitutionService;
        private readonly IUserService _iUserService;

        public ManageController(IInstitutionService iInstitutionService, IUserService iUserService)
        {
            _iInstitutionService = iInstitutionService;
            _iUserService = iUserService;

        }
        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IInstitutionService instNameService, IUserService iUserService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _iInstitutionService = instNameService;
            _iUserService = iUserService;
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
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
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

        public void Initialize()
        {
            string userId = User.Identity.GetUserId();
            //var InsId = _iInstitutionService.GetInstName(userId);
            //ViewBag.Institution = new SelectList(InsId, "InstId", "InstitutionName");

            //List<IncidentManagementSystem.Model.Roles> role = _iInstitutionService.RoleList();
            //ViewBag.UserRole = new SelectList(role, "Id", "Name");
            List<IncidentManagementSystem.Model.Roles> role = _iInstitutionService.RoleList();
            ViewBag.UserRole = new SelectList(role, "Name", "Name");

        }

        //Custom Profile Edit //
        // GET: /Manage/Index
        [HttpGet]
        public async Task<ActionResult> ProfileEdit(string userId)
        {
            Initialize();
            var user = await UserManager.FindByIdAsync(userId);


            var currentRoles = await UserManager.GetRolesAsync(user.Id);

            var model = new EditProfileViewModel
            {
                UserName = user.UserName,
                Id = user.Id,
                UserRole_Id = currentRoles.FirstOrDefault(),
            };
            return View(model);
        }

        //POST: /Manage/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProfileEdit(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    //update username
                    if ((!string.IsNullOrEmpty(model.NewUserName)) && model.NewUserName != user.UserName)
                    {
                        user.UserName = model.NewUserName;
                        var userUpdateResult = await UserManager.UpdateAsync(user);
                        if (!userUpdateResult.Succeeded)
                        {
                            // Handle errors if user update failed
                            foreach (var error in userUpdateResult.Errors)
                            {
                                ModelState.AddModelError("", error);
                            }
                            ViewBag.TaskStatus = "99"; // Indicating failure
                            ViewBag.TaskMessage = "Failed to update username.";
                            return View(model); // Return the view with the model containing the errors
                        }
                        else
                        {
                            ViewBag.TaskStatus = "00"; // Indicating success
                            ViewBag.TaskMessage = "Username updated successfully!";
                        }
                    }
                    //update password
                    if (model.ShowPartialView && !string.IsNullOrEmpty(model.Password))
                    {
                        UserManager.RemovePassword(model.Id);
                        var result = await UserManager.AddPasswordAsync(user.Id, model.Password);
                        if (result.Succeeded)
                        {
                            ViewBag.TaskStatus = "00"; // Indicating success
                            ViewBag.TaskMessage = "Password updated successfully!";
                            //return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ViewBag.TaskStatus = "99"; // Indicating failure
                            ViewBag.TaskMessage = "Failed to update password.";
                            return View(model);
                        }
                    }
                    //Update Role
                    if (!string.IsNullOrEmpty(model.UserRoleId))
                    {
                        var removeResult = await UserManager.RemoveFromRolesAsync(user.Id, model.UserRole_Id);

                        var Result = await UserManager.AddToRoleAsync(user.Id, model.UserRoleId);
                        if (!Result.Succeeded)
                        {
                            foreach (var error in Result.Errors)
                            {
                                ModelState.AddModelError("", error);
                            }
                            ViewBag.TaskStatus = "99";
                            ViewBag.TaskMessage = "Failed to add new role.";
                            return View(model);
                        }
                        else
                        {
                            ViewBag.TaskStatus = "00";
                            ViewBag.TaskMessage = "Role updated successfully!";
                        }
                    }
                  
                }
                else
                {
                    ModelState.AddModelError("", "User not found.");
                }

            }
            Initialize();
            ViewBag.TaskStatus = "00";
            ViewBag.TaskMessage = "Profile updated successfully!";
            //await _userManager.UpdateAsync(user);
            return View(model);
        }

        //[HttpGet]
        //public async Task<ActionResult> EditUser(string userId)
        //{
        //    Initialize();
        //    var user = await UserManager.FindByIdAsync(userId);

        //    var model = new EditProfileViewModel
        //    {
        //        UserName = user.UserName,
        //        //Id = userId,
        //        Id = user.Id,
        //    };
        //    //ViewBag.userId = userId;
        //    return PartialView("EditUser", model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> EditUser(EditProfileViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return PartialView("EditUser", model); // Return the view with validation errors
        //    }

        //    var user = await _userManager.FindByIdAsync(model.Id);
        //    user.UserName = model.UserName;
        //    // If a password is provided, update it
        //    if (!string.IsNullOrEmpty(model.Password))
        //    {
        //        var passwordUpdateResult = await UpdatePasswordBySuperAdmin(UserManager, user, model.Password, User as ClaimsPrincipal);
        //        if (!passwordUpdateResult.Succeeded)
        //        {
        //            foreach (var error in passwordUpdateResult.Errors)
        //            {
        //                ModelState.AddModelError(string.Empty, error);
        //            }
        //            return PartialView("EditUser", model); // Return with errors
        //        }
        //    }

        //    // Update the user in the database
        //    var updateResult = await _userManager.UpdateAsync(user);
        //    if (!updateResult.Succeeded)
        //    {
        //        foreach (var error in updateResult.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error);
        //        }
        //        return PartialView("EditUser", model); // Return with errors
        //    }

        //    TempData["SuccessMessage"] = "User updated successfully.";
        //    return RedirectToAction("Index"); // Redirect to an appropriate page
        //}

        //private async Task<IdentityResult> UpdatePasswordBySuperAdmin(UserManager<ApplicationUser> userManager, ApplicationUser user, string newPassword, ClaimsPrincipal currentUser)
        //{
        //    // Remove the existing password
        //    var token = await userManager.GeneratePasswordResetTokenAsync(user.Id);
        //    return await userManager.ResetPasswordAsync(user.Id, token, newPassword);
        //}

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
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
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
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