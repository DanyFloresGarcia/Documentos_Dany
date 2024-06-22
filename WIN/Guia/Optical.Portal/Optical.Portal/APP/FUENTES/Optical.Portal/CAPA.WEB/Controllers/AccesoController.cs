using CAPA.MODELO;
using CAPA.UTIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAPA.WEB.Controllers
{
    public class AccesoController : AppController
    {
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Aplicacion");
            }
            else
            {
                Implementacion.DeleteCookie("LoggedNombreUsuario");
                Implementacion.SetSession("UsuarioSesion", null);
                return RedirectToAction("Index", "Login",new { returnUrl });
            }
        }

        public ActionResult Index(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Aplicacion");
            }
            else
            {
                Implementacion.DeleteCookie("LoggedNombreUsuario");
                Implementacion.SetSession("UsuarioSesion", null);
                return RedirectToAction("Index", "Login", new { returnUrl });
            }
        }
    }
}