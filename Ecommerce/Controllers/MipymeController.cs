using Ecommerce.DAL;
using Ecommerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ecommerce.ApplicationSignInManager;

namespace Ecommerce.Controllers
{
    //[Authorize(Users = "10b67bc1-b50d-4c8a-b59c-2b4603fad3a8")]
    public class MipymeController : Controller
    {


        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        dbEcommerceEntities1 ctx = new dbEcommerceEntities1();

        // GET: Mipyme
        public ActionResult Index()
        {
            //UserManager.AddToRole("54f5e4f3-2b6c-4430-a871-73b386aa9b4a", "MiPymes");

            List<UserViewModel> usuarios = ctx.AspNetUsers.Where(aspnetUser => aspnetUser.AspNetRoles.Count(netRole => netRole.Name.Contains("MiPymes")) > 0).Select(aspnetUser => new UserViewModel
            {
                Name = aspnetUser.UserName,
                Email = aspnetUser.Email
            }).ToList();
            return View(usuarios);
        }

        // GET: /Account/Register
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
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
        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }

        }
        //
        // POST: /MiPyme/Register  
        [AllowAnonymous]
        //[HttpPost]
        public async Task<ActionResult> Register(RegisterMiPymeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.MiPymeName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, "Zt3{77[H");
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "MiPymes");
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // Para obtener más información sobre cómo habilitar la confirmación de cuentas y el restablecimiento de contraseña, visite https://go.microsoft.com/fwlink/?LinkID=320771
                    // Enviar correo electrónico con este vínculo
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirmar cuenta", "Para confirmar la cuenta, haga clic <a href=\"" + callbackUrl + "\">aquí</a>");

                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}