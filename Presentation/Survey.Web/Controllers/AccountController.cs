using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Survey.Web.Resources;
using System.Web;
using Microsoft.Owin.Security;
using Castle.Core.Logging;

using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Survey.Business.Services.Contracts;
using Survey.Web.ViewModel;
using System.DirectoryServices.AccountManagement;
using Survey.Business.Entities.Enums;

namespace Survey.Web.Controllers
{
    public class AccountController : Controller
    {

        private ILogger _logger;
        private IActiveDirectoryService _activeDirectoryService;

        #region     Public  Method
        public AccountController(ILogger logger, IActiveDirectoryService activeDirectoryService)
        {
            _activeDirectoryService = activeDirectoryService;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            LoginModel model = new LoginModel();
            List<SelectListItem> DomainName = new List<SelectListItem>();
            DomainName.Add(new SelectListItem { Text = "Corporate / Head Office", Value = "corp" });
            DomainName.Add(new SelectListItem { Text = "ValueLabs SEZ", Value = "lsz" });
            DomainName.Add(new SelectListItem { Text = "ValueLabs Solutions", Value = "SLS" });
            DomainName.Add(new SelectListItem { Text = "ValueLabs Technologies Unit – 2", Value = "T02" });
            DomainName.Add(new SelectListItem { Text = "ValueLabs Technologies – LLP", Value = "T03" });
            DomainName.Add(new SelectListItem { Text = "ValueLabs Solutions", Value = "TLP" });
            DomainName.Add(new SelectListItem { Text = "PHP / Cebu Office", Value = "php" });
            ViewBag.Domain = DomainName;
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult AdminLogin()
        {
            LoginModel model = new LoginModel();
            model.IsAdmin = true;

            List<SelectListItem> DomainName = new List<SelectListItem>();
            DomainName.Add(new SelectListItem { Text = "Corporate / Head Office", Value = "corp" });
            DomainName.Add(new SelectListItem { Text = "ValueLabs SEZ", Value = "lsz" });
            DomainName.Add(new SelectListItem { Text = "ValueLabs Solutions", Value = "SLS" });
            DomainName.Add(new SelectListItem { Text = "ValueLabs Technologies Unit – 2", Value = "T02" });
            DomainName.Add(new SelectListItem { Text = "ValueLabs Technologies – LLP", Value = "T03" });
            DomainName.Add(new SelectListItem { Text = "ValueLabs Solutions", Value = "TLP" });
            DomainName.Add(new SelectListItem { Text = "PHP / Cebu Office", Value = "php" });
            ViewBag.Domain = DomainName;
            return View("Login", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            SignOut();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, ModelValidations.Account_InvalidLoginError);
                return View(model);
            }


            var applicationUserClaims = await _activeDirectoryService.ValidateUser(model.DomainName, model.UserName, model.Password);

            //using (var pc = new PrincipalContext(ContextType.Domain, "10.10.52.100"))
            //{
            //    var isValid = pc.ValidateCredentials(model.UserName, model.Password);

            //}
            if (applicationUserClaims != null)
            {
                //Add Role as User Or Admin
                if (model.IsAdmin)
                {
                    //valdiate that It is admin in table

                    applicationUserClaims.Add(new Claim(ClaimTypes.Role, UserRole.Admin.ToString()));
                }
                else
                {
                    applicationUserClaims.Add(new Claim(ClaimTypes.Role, UserRole.User.ToString()));
                }

                var claimsIdentity = new ClaimsIdentity(applicationUserClaims, DefaultAuthenticationTypes.ApplicationCookie);

                ControllerContext.HttpContext.GetOwinContext()
               .Authentication
               .SignIn(new AuthenticationProperties() { IsPersistent = true }, claimsIdentity);


                if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                {

                    return Redirect(returnUrl);
                }

                _logger.Info("user logged in");
                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }

            else
            {
                ModelState.AddModelError(string.Empty, ModelValidations.Account_InvalidLoginMessage);
                ModelState.AddModelError(string.Empty, ModelValidations.Account_InvalidLoginMessage1);
            }
            return View(model);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            SignOut();
            _logger.Info("user SignOut");
            return RedirectToAction("Login");
        }


        #endregion


        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        private void SignOut()
        {
            IAuthenticationManager authenticationManager = ControllerContext.HttpContext.GetOwinContext().Authentication;
            IEnumerable<string> authenticationTypes = authenticationManager.GetAuthenticationTypes().Select(ad => ad.AuthenticationType);
            authenticationManager.SignOut(new AuthenticationProperties() { RedirectUri = string.Empty }, authenticationTypes.ToArray());
        }

        #endregion
    }
}