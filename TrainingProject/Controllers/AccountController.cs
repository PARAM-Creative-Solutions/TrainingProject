using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TrainingProject.Models;
using TrainingProject.Security;
using Newtonsoft.Json;
using System.Web.Security;
using Microsoft.AspNet.SignalR;
using System.IO;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using System.Data.Entity;
using System.Collections.Generic;
using TrainingProjectDataLayer.Constants;
using TrainingProject.Models.MailManager;

namespace TrainingProject.Controllers
{
    /// <summary>
    /// Manages the login and logout of user account
    /// </summary>
    [CustomErrorHandler]
    public class AccountController : Controller
    {

        #region Properties
        /// <summary>
        /// Unit Of Work Object
        /// </summary>
        public UnitofWork uow { get; set; }

        /// <summary>
        /// CurrentUser
        /// </summary>
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.User as CustomPrincipal; }
        }

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates the instance of Unit of work 
        /// </summary>
        public AccountController()
        {
            uow = new UnitofWork(new TrainingProjectEntities());
        }
        #endregion

        /// <summary>
        /// AccountController
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        /// <summary>
        /// SignInManager
        /// </summary>
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

        /// <summary>
        /// UserManager
        /// </summary>
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

        #region Login

        #region HTTPGET Login

        /// <summary>
        /// Gets the credentials from user
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            if (SessionPersister.CurrentUser != null)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            ViewBag.Departments = new SelectList(uow.DepartmentRepository.GetAll(), "DeptId", "Name");
            LoginViewModel vm = new LoginViewModel();
            return View(vm);
        }

        #endregion

        #region Login HTTPPOST

        /// <summary>
        /// Submits the form of login to server
        /// </summary>
        /// <param name="pModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginViewModel pModel)
        {
            if (ModelState.IsValid)
            {
                string EncryptedPassword = Security.CustomEncryption.Encrypt(pModel.Password);
                User user = uow.UserRepository.ValidateUser(new User { UserName = pModel.Username, Password = EncryptedPassword, DeptId = pModel.Department });
                // string name = Request.UserHostName;
                if (user != null)
                {
                    if (user.UserStatus == (int)SystemEnums.UserStatus.Active)
                    {
                        TimeSpan? dt = DateTime.Today - user.PasswordLastModifiedOn; // calculation the difference between today's date and password last modified on added by priyanka
                    if (dt < TimeSpan.FromDays(60) || user.PasswordLastModifiedOn != null) // if time span is greater than 60 days and password last modified date is null added by priyanka
                    {
                        List<UserLog> onlineuserLogs = user.UserLogs.Where(w => w.OnlineStatus).ToList(); //us.UserLogs.FirstOrDefault();  
                        #region Create User Log
                        if (onlineuserLogs.Count >= 1)
                        {
                            #region Execute After user Login From Another System
                            ViewBag.UserId = user.UserId;
                            ViewBag.LoggedInSystem = string.Join(",", onlineuserLogs.Select(s => s.IPAddress));
                            ViewBag.Departments = new SelectList(uow.DepartmentRepository.GetAll(), "DeptID", "DeptName");
                            #endregion
                            return View(pModel);
                        }
                        else
                        {
                            Response.Cookies.Add(CreateHttpCookie(user, pModel.Remember));
                        }
                        #endregion
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                    else
                    {
                        //if password need to be updated in 60 days then this will execute 
                        //added by priyanka
                        //added on 5 - 09 - 2019
                        return View("ChangePassword", user);
                    }
                }
                    else
                    {
                        ViewBag.Message = "User Is Inactive !";
                        pModel.Password = string.Empty;
                        ViewBag.Departments = new SelectList(uow.DepartmentRepository.GetAll(), "DeptId", "Name");
                        return View("Login", pModel);
                    }
                }
                else
                {
                    ViewBag.Message = "Username Or Password Is Incorrect !";
                    pModel.Password = string.Empty;
                    ViewBag.Departments = new SelectList(uow.DepartmentRepository.GetAll(), "DeptId", "Name");
                    return View("Login", pModel);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        #endregion

        /// <summary>
        /// Create Http Cookies for login user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="RememberMe"></param>
        /// <returns></returns>
        [NonAction]
        public HttpCookie CreateHttpCookie(User user, bool RememberMe = false)
        {
            #region Create Login User Information As Token In cookies
            string SessionId = HttpContext.Session.SessionID;
            //**********added on 01-01-2018 by vikrant to get and update the system information of the user system
            string clientMachineName = HelperMethods.GetHostDetails(HttpContext); //System.Web.HttpContext.Current.Request.UserHostAddress;
            string CLientIP_Address = HelperMethods.GetIPAddress(HttpContext);
            SystemDetail systemDetails = uow.SystemDetailRepository.GetAll().FirstOrDefault();
            UserLog onlineuserLog = new UserLog
            {
                IPAddress = CLientIP_Address,
                SystemInfo = clientMachineName,
                UserId = user.UserId,
                OnlineStatus = true,
                LoggedInTime = DateTime.Now,
                SessionId = SessionId,
                ConnectionId = ""
            };
            uow.UserLogsRepository.Add(onlineuserLog);
            uow.SaveChanges();

            List<string> Roles = uow.UserRoleRepository.GetAll(r => r.UserId == user.UserId).Select(s => s.Role.RoleName).ToList();
            List<int> Rights = uow.RoleWiseRightRepository.GetAll(r => Roles.Contains(r.Role.RoleName)).Select(s => s.RightId).Distinct().ToList();
            CustomPrincipal serializeModel = new CustomPrincipal(new System.Security.Principal.GenericIdentity(user.UserName))
            {
                FullName = user.FirstName + " " + user.LastName,
                Id = user.UserId,
                PlantId = user.PlantId,
                DepartmentId = user.DeptId,
                Rights = Rights,
                SessionId = SessionId.ToString(),
            };
         
            #region Add User Info In Cookies

            string userData = JsonConvert.SerializeObject(serializeModel);
            double timeout = RememberMe ? systemDetails.RememberMeMinutes : FormsAuthentication.Timeout.TotalMinutes; // Timeout in minutes, 525600 = 365 days.
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, user.UserName, DateTime.Now, DateTime.Now.AddMinutes(timeout), RememberMe, userData);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            faCookie.Expires = DateTime.Now.AddMinutes(timeout);
            faCookie.Secure = systemDetails.SSL;
            faCookie.HttpOnly = systemDetails.HttpOnlyCookies;
            faCookie.Shareable= systemDetails.ShareableCookies;
            return faCookie;

            #endregion

            #endregion
        }


        #endregion

        #region ContinueLogin
        /// <summary>
        /// ContinueLogin
        /// </summary>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public JsonResult ContinueLogin(LoginViewModel pModel)
        {
            string EncryptedPassword = Security.CustomEncryption.Encrypt(pModel.Password);
            User user = uow.UserRepository.ValidateUser(new User { UserName = pModel.Username, Password = EncryptedPassword, DeptId = pModel.Department });
            if (user != null)
            {
                List<UserLog> UserLogs = uow.UserLogsRepository.GetAll(w => w.UserId == user.UserId && w.OnlineStatus).ToList();
                foreach (UserLog userLog in UserLogs)
                {
                    MyApplicationHub.LogOutUser(userLog.ConnectionId);
                    userLog.OnlineStatus = false;
                }
                uow.SaveChanges();
                Response.Cookies.Add(CreateHttpCookie(user, pModel.Remember));

                return Json(new { Result = true, RedirectUrl = "/Dashboard/Dashboard" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = false, RedirectUrl = "/Account/Login", Message = "Invalid UserName Or Password !" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        
        #region Forgot password
        #region Get
        /// <summary>
        /// ForgotPassword
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        #endregion

        #region Post
        /// <summary>
        /// Method password recovery and send password to user by Email
        /// </summary>
        /// <param name="userName">Username of user for password recovery</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string userName)
        {
            Models.BLL.BL_Email emailbl = new Models.BLL.BL_Email();
            emailbl.ForgotPassword(userName, ControllerContext, this.ViewData);

            //var user = uow.UserRepository.Get(x => x.UserName.ToUpper() == userName.ToUpper());
            //if (user != null)
            //{
            //    var userEmail = user.EmailId;
            //    var password = CustomEncryption.Decrypt(user.Password);
            //    if (userEmail != null)
            //    {
            //        await Mail.SendMail(new MailData { Body = "Username : " + user.UserName + "\n Password : " + password, Subject = "Password Recovery", To = userEmail });
            //        return Json(new { Result = true, Message = "Password Has Been Send To Your Email !" }, JsonRequestBehavior.AllowGet);
            //    }
            //    else
            //    {
            //        return Json(new { Result = false, Message = "Email Id Does Not Exists !" }, JsonRequestBehavior.AllowGet);
            //    }
            //}
            // ViewBag.Message = "Username Does Not Exists";
            return Json(new { Result = emailbl.Result, Message = emailbl.Message }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Change Password

        /// <summary>
        /// Method for getting view of new password
        /// </summary>
        /// added on 12-02-2019 by priyanka
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Method which reflect the post action for changing password
        /// </summary>
        /// created by priyanka for updating password in every 60 days
        /// added on 05-09-2019
        /// <param name="UserId"></param>
        /// <param name="OldPassword"></param>
        /// <param name="NewPassword"></param>
        /// <param name="ConfirmPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ChangePassword(int UserId, string OldPassword, string NewPassword, string ConfirmPassword)
        {
            User user = uow.UserRepository.Get(x => x.UserId == UserId);
            if (CustomEncryption.Decrypt(user.Password) == OldPassword)
            {
                if (NewPassword == ConfirmPassword && NewPassword != "" && ConfirmPassword != "")
                {
                    user.Password = CustomEncryption.Encrypt(NewPassword);
                    user.PasswordLastModifiedOn = DateTime.Now;
                    uow.UserRepository.Update(user);
                    uow.SaveChanges();
                    return Json(new { Result = true, Message = "Password Has Been Updated !" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (NewPassword == "") { return Json(new { Result = false, Message = "Please Fill Valid New Password  !" }, JsonRequestBehavior.AllowGet); }
                    if (ConfirmPassword == "") { return Json(new { Result = false, Message = "Please Fill Valid Confirm Password  !" }, JsonRequestBehavior.AllowGet); }
                    return Json(new { Result = false, Message = "New Password And Conirm Password Does Not Match !" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Result = false, Message = "Old Password Is Not Valid !" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region CheckCookies
        private User CheckCookies()
        {
            User oUser = null;
            string UserName = string.Empty; string Password = string.Empty;
            if (Request.Cookies["UserName"] != null)
                UserName = Request.Cookies["UserName"].Value;
            if (Request.Cookies["Password"] != null)
                Password = Request.Cookies["Password"].Value;
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                oUser = new User { UserName = UserName, Password = Password };
            return oUser;
        }
        #endregion

        #region Commented by vik For LOGIN
        ////
        //// POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // This doesn't count login failures towards account lockout
        //    // To enable password failures to trigger account lockout, change to shouldLockout: true
        //    var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.Remember, shouldLockout: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.Remember });
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("", "Invalid login attempt.");
        //            return View(model);
        //    }
        //}

        //
        // GET: /Account/VerifyCode
        #endregion

        #region HTTPGET Logout

        /// <summary>
        /// Log out from account
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.Authorize]
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            CustomPrincipal cp = HttpContext.User as CustomPrincipal;
            if (cp != null)
            {
                string clientMachineName = HelperMethods.GetHostDetails(HttpContext);
                string CLientIP_Address = HelperMethods.GetIPAddress(HttpContext);
                UserLog uo = uow.Db.UserLogs.Where(x => x.SessionId.ToString() == cp.SessionId).FirstOrDefault();
                if (uo != null)
                {
                    uo.LastAcessedOn = DateTime.Now;
                    uo.OnlineStatus = false;
                    uo.IPAddress = CLientIP_Address;
                    uo.LoggedOutTime = DateTime.Now;
                    uo.SystemInfo = clientMachineName;
                    uo.IsOfflineByAdmin = false;
                    uow.Db.UserLogs.Attach(uo);
                    uow.Db.Entry(uo).State = EntityState.Modified;
                    uow.SaveChanges();
                }
               MyApplicationHub.UpdateOnlineUsersCount();
            }

            if (Request.Cookies["UserName"] != null)
            {
                HttpCookie ockUserName = new HttpCookie("UserName");
                ockUserName.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ockUserName);
            }
            if (Request.Cookies["Password"] != null)
            {
                HttpCookie ockPassword = new HttpCookie("Password");
                ockPassword.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ockPassword);
            }
            return RedirectToAction("Login");
        }

        #endregion

        /// <summary>
        /// VerifyCode
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        /// <summary>
        /// VerifyCode
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// ConfirmEmail
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        /// <summary>
        /// ResetPassword
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        /// <summary>
        /// ResetPassword
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        /// <summary>
        /// ResetPasswordConfirmation
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// ExternalLogin
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        /// <summary>
        /// SendCode
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        /// <summary>
        /// SendCode
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        /// <summary>
        /// ExternalLoginCallback
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        /// <summary>
        /// ExternalLoginConfirmation
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        /// <summary>
        /// LogOff
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// ExternalLoginFailure
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        /// <summary>
        /// Opens the user manual in new tab
        /// </summary>
        /// <returns></returns>
        public ActionResult UserManual(SystemEnums.UserManualTypes type)
        {
            try
            {
                string filePath = Server.MapPath(Path.Combine("~/App_Data/User Manual",HelperMethods.GetDescription(type)));
                if (System.IO.File.Exists(filePath))
                {
                    return new FileStreamResult(new FileStream(filePath, FileMode.Open, FileAccess.Read), filePath);
                }
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}