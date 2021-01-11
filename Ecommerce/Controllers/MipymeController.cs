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

namespace Ecommerce.Controllers
{
    public class MipymeController : Controller
    {
        List<string> adminUsers = new List<string> {
        ""
        };
        List<UserViewModel> usuariosMiembros = new List<UserViewModel> {
        new UserViewModel  
            {
                    Name = "Beestore",
                    Email = "beestore@gmail.com"
             }
        };
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        dbEcommerceEntities1 ctx = new dbEcommerceEntities1();

        // GET: Mipyme
        public ActionResult Index()
        {
            List<UserViewModel> usuarios = ctx.AspNetUsers.Where(aspnetUser => adminUsers.Contains(aspnetUser.Id)).Select(aspnetUser => new UserViewModel
            {
                Name = aspnetUser.UserName,
                Email = aspnetUser.Email
            }).ToList();
            return View(usuariosMiembros);
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
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // Para obtener más información sobre cómo habilitar la confirmación de cuentas y el restablecimiento de contraseña, visite https://go.microsoft.com/fwlink/?LinkID=320771
                    // Enviar correo electrónico con este vínculo
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirmar cuenta", "Para confirmar la cuenta, haga clic <a href=\"" + callbackUrl + "\">aquí</a>");

                    return RedirectToAction("Index", "Home");
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