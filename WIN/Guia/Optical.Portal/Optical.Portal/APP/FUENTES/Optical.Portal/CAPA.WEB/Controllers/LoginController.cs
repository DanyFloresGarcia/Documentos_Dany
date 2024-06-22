using CAPA.MODELO;
using CAPA.NEGOCIO;
using CAPA.UTIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CAPA.WEB.Controllers
{
    public class LoginController : AppController
    {
        LoginBL loginBL;

        public LoginController()
        {
            loginBL = new LoginBL();
        }

        // GET: Login
        public ActionResult Index(string returnUrl)
        {
            return LoggedUser(() => {
                Implementacion.SetSession("ReturnUrl", returnUrl);
                return View();
            });
        }

        public ActionResult Autenticar(Login login, string returnUrl)
        {
            ResultadoWeb resultadoConsulta = new ResultadoWeb();
            #region Código programable
            resultadoConsulta = loginBL.Autenticar(login);
            Implementacion.SetSession("ReturnUrl", string.Empty);
            #endregion
            return JsonController(resultadoConsulta);
        }

        public ActionResult CerrarSesion()
        {
            System.Web.HttpContext.Current.Session["CurrentUser"] = null;
            Implementacion.LimpiarSesion();

            return RedirectToAction("Index", "Login");
        }
    }
}