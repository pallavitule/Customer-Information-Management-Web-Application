using Microsoft.AspNetCore.Mvc;
using MVCDHProject5.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System.Security.Claims;
//using System.Net.Mail;
using MailKit.Security;
using NuGet.Common;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
namespace MVCDHProject5.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager; // Add this field
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager; // Assign the parameter to the field
            this.signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser identityUser = new IdentityUser
                {
                    UserName = userModel.Name,
                    Email = userModel.Email,
                    PhoneNumber = userModel.Mobile
                };

                var result = await userManager.CreateAsync(identityUser, userModel.Password);
                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    var confirmationUrlLink = Url.Action("ConfirmEmail", "Account", new { UserId = identityUser.Id, Token = token }, Request.Scheme);
                    //Passing the information toSendMail methodtosendthe Mail
                    SendMail(identityUser, confirmationUrlLink, "Email Confirmation Link");
                    TempData["Title"] = "Email Confirmation Link";
                    TempData["Message"] = "Aconfirmemaillink has been sent to your registered mail, click onittoconfirm.";
                    return View("DisplayMessages");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(userModel);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            //Code tocheck whetherEmailis confirmedornot
            var user = await userManager.FindByNameAsync(loginModel.Name);
            if (user != null && (await userManager.CheckPasswordAsync(user, loginModel.Password)) &&
            user.EmailConfirmed == false)
            {
                ModelState.AddModelError("", "Your email is not confirmed.");
                return View(loginModel);
            }
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginModel.Name, loginModel.Password,
                loginModel.RememberMe, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginModel.ReturnUrl))
                        return RedirectToAction("Index", "Home");
                    else
                        return LocalRedirect(loginModel.ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login credentials.");
                }
            }
            return View(loginModel);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public void SendMail(IdentityUser identityUser, string requestLink, string subject)
        {
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append("Hello " + identityUser.UserName + "<br/><br/>");

            if (subject == "Email Confirmation Link")
            {
                mailBody.Append("Click on the link below to confirm your email:");
            }
            else if (subject == "Change PasswordLink")
            {
                mailBody.Append("Click on the link below to reset your password:");
            }
            mailBody.Append("<br />");
            mailBody.Append(requestLink);
            mailBody.Append("<br /><br/> ");
            mailBody.Append("Regards,");
            mailBody.Append("<br /><br/>");
            mailBody.Append("Customer Support.");

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = mailBody.ToString();

            // ✅ Replace with your actual email
            MailboxAddress fromAddress = new MailboxAddress("Customer Support", "pallavitule00@gmail.com");
            MailboxAddress toAddress = new MailboxAddress(identityUser.UserName, identityUser.Email);

            MimeMessage mailMessage = new MimeMessage();
            mailMessage.From.Add(fromAddress);
            mailMessage.To.Add(toAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = bodyBuilder.ToMessageBody();

            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtpClient.Authenticate("pallavitule60@gmail.com", "Vasu@1965");
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
            }
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId != null && token != null)
            {
                var User = await userManager.FindByIdAsync(userId);
                if (User != null)
                {
                    var result = await userManager.ConfirmEmailAsync(User, token);
                    if (result.Succeeded)
                    {
                        TempData["Title"] = "Email Confirmation Success.";
                        TempData["Message"] = "Email confirmationis completed.Youcannow loginintothe application.";
                        return View("DisplayMessages");
                    }
                    else
                    {
                        StringBuilder Errors = new StringBuilder();
                        foreach (var Error in result.Errors)
                        {
                            Errors.Append(Error.Description + ". ");
                        }
                        TempData["Title"] = "Confirmation Email Failure";
                        TempData["Message"] = Errors.ToString();
                        return View("DisplayMessages");
                    }
                }
                else
                {
                    TempData["Title"] = "Invalid User Id.";
                    TempData["Message"] = "UserIdwhichis presentinconfirmemail link is in-valid.";
                    return View("DisplayMessages");
                }
            }
            else
            {
                TempData["Title"] = "Invalid Email Confirmation Link.";
                TempData["Message"] = "Email confirmationlink is invalid, eithermissingthe UserIdorConfirmation Token.";
                return View("DisplayMessages");
            }
        }

            [HttpGet]
            public IActionResult ForgotPassword()
            {
                return View();
            }
            [HttpPost]
            public async Task<IActionResult> ForgotPassword(ChangePasswordModel model)
            {
                if (ModelState.IsValid)
                {
                    var User = await userManager.FindByNameAsync(model.Name);
                    if (User != null && await userManager.IsEmailConfirmedAsync(User))
                    {
                        var token = await userManager.GeneratePasswordResetTokenAsync(User);
                        var confirmationUrlLink = Url.Action("ChangePassword", "Account", new
                        {
                            UserId = User.Id,
                            Token =
                        token
                        },
                        Request.Scheme);
                        SendMail(User, confirmationUrlLink, "Change PasswordLink");
                        TempData["Title"] = "Change PasswordLink";
                        TempData["Message"] = "Change passwordlink has beensenttoyourmail, click onitandchange password.";
                         return View("DisplayMessages");
                    }
                    else
                    {
                        TempData["Title"] = "Change PasswordMail Generation Failed.";
                        TempData["Message"] = "Eitherthe Username youhave enteredis in-validoryouremail is notconfirmed.";
                        return View("DisplayMessages");
                    }
                }
                return View(model);
            }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await userManager.FindByIdAsync(model.UserId);
                if (User != null)
                {
                    var result = await userManager.ResetPasswordAsync(User, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        TempData["Title"] = "Reset PasswordSuccess";
                        TempData["Message"] = "Yourpasswordhas beenresetsuccessfully.";
                        return View("DisplayMessages");
                    }
                    else
                    {
                        foreach (var Error in result.Errors)
                            ModelState.AddModelError("", Error.Description);
                    }
                }
                else
                {
                    TempData["Title"] = "Invalid User";
                    TempData["Message"] = "Nouserexists withthe givenUserId.";
                    return View("DisplayMessages");
                }
            }
            return View(model);
        }

        public IActionResult ExternalLogin(string returnUrl, string Provider)
        {
            var url = Url.Action("CallBack", "Account", new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(Provider, url);
            return new ChallengeResult(Provider, properties);
        }
        public async Task<IActionResult> CallBack(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "~/";
            }
            LoginViewModel model = new LoginViewModel();
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError("", "Error loading external login information.");
                return View("Login", model);
            }
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false,
            true);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = await userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new IdentityUser{ UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                        PhoneNumber = info.Principal.FindFirstValue(ClaimTypes.MobilePhone),
                         };
                        var identityResult = await userManager.CreateAsync(user);
                    }
                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, false);
                    return LocalRedirect(returnUrl);
                }
                TempData["Title"] = "Error";
                TempData["Message"] = "Email claimnotreceivedfromthirdparty provided.";
                return RedirectToAction("DisplayMessages");
            }
        }
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null && !user.EmailConfirmed)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationUrlLink = Url.Action("ConfirmEmail", "Account", new { UserId = user.Id, Token = token }, Request.Scheme);
                SendMail(user, confirmationUrlLink, "Email Confirmation Link");
                TempData["Message"] = "A new confirmation email has been sent.";
            }
            else
            {
                TempData["Message"] = "Invalid request or email already confirmed.";
            }
            return View("DisplayMessages");
        }

    }
}

