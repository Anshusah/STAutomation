using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Cicero.Data.Entities;
using Microsoft.AspNetCore.Http;
using Cicero.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Cicero.Service.Library.Toastr;

namespace Cicero.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _IHttpContextAccessor = null;
        private readonly IToastNotification _toastNotification;
        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, ApplicationDbContext db, 
            UserManager<ApplicationUser> userManager, IHttpContextAccessor iHttpContextAccessor, IToastNotification toastNotification)
        {
            _signInManager = signInManager;
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _IHttpContextAccessor = iHttpContextAccessor;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/admin.html");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {

                    var loggedUser = _db.Users.Where(x => x.Email == Input.Email).Select(z => new {z.Id, z.IsSuperAdmin }).FirstOrDefault();
                    if (loggedUser.IsSuperAdmin == false)
                    {
                        string tenantIdentifier = await _db.TenantUser
                                                    .Where(x => x.UserId == loggedUser.Id)
                                                    .Include(y => y.TenantForUser).Select(b =>
                                                    b.TenantForUser.Identifier).FirstOrDefaultAsync();
                        if (!string.IsNullOrEmpty(tenantIdentifier))
                        {
                            HttpContext.Session.SetString("tenant_identifier", tenantIdentifier);
                            var check = HttpContext.Session.GetString("tenant_identifier");
                        }
                    }


                    _logger.LogInformation("User logged in.");
                    _toastNotification.AddSuccessToastMessage("Logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    _toastNotification.AddWarningToastMessage("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    _toastNotification.AddErrorToastMessage("Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
