using CAPA.NEGOCIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAPA.WEB.Controllers
{
    public class UsuarioController : AppController
    {
        UsuarioBL usuarioBL;

        public UsuarioController()
        {
            usuarioBL = new UsuarioBL();
        }

        [HttpPost]
        public ActionResult DatosUsuarioSesion()
        {
            return JsonController(usuarioBL.DatosUsuarioSesion());
        }
    }
}