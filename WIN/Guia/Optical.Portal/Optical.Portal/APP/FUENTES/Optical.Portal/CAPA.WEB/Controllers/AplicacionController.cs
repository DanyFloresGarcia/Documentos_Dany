using CAPA.MODELO;
using CAPA.NEGOCIO;
using CAPA.UTIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAPA.WEB.Controllers
{
    public class AplicacionController : AppController
    {
        AplicacionBL aplicacionBL;

        public AplicacionController()
        {
            aplicacionBL = new AplicacionBL();
        }

        // GET: Aplicacion
        public ActionResult Index()
        {
            return ViewController(() => {
                Implementacion.SetSession("ReturnUrl", string.Empty);
                return View();
            },
            new DatosControlador()
            {
                NombreControlador = ControllerContext.RouteData.Values["controller"].ToString(),
                NombreMetodo = ControllerContext.RouteData.Values["action"].ToString()
            });
        }

        [HttpPost]
        public ActionResult ListaPorNombreUsuario()
        {
            return JsonController(aplicacionBL.ListaPorNombreUsuario());
        }
    }
}