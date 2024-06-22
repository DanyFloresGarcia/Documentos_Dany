using CAPA.MODELO;
using CAPA.NEGOCIO;
using CAPA.UTIL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace CAPA.WEB.Controllers
{
    public class AppController : Controller
    {
        AppBL appBL;

        public AppController()
        {
            appBL = new AppBL();
        }

        public ActionResult LoggedUser(Func<ActionResult> func)
        {
            ResultadoWeb resultadoWeb = new ResultadoWeb();
            ViewBag.VersionSistema = Implementacion.GetConfigKey<string>("VersionSistema");

            if (appBL.ViewController().EstadoSolicitud.EstaCorrecto == true)
            {
                return RedirectToAction("Index", "Aplicacion");
            }
            else
            {
                return func();
            }
        }

        public ActionResult ViewController(Func<ActionResult> func, DatosControlador datosControlador)
        {
            ViewBag.VersionSistema = Implementacion.GetConfigKey<string>("VersionSistema");

            if (appBL.ViewController().EstadoSolicitud.EstaCorrecto == false)
            {
                return RedirectToAction("CerrarSesion", "Login");
            } else
            {
                return func();
            }
        }

        public ActionResult JsonController(ResultadoWeb resultadoConsulta)
        {
            return Json(JsonConvert.SerializeObject(resultadoConsulta, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }

        [HttpPost]
        public ActionResult Variables(AplicacionConfiguracion aplicacionConfiguracion)
        {
            return JsonController(appBL.Variables(aplicacionConfiguracion));
        }
    }
}