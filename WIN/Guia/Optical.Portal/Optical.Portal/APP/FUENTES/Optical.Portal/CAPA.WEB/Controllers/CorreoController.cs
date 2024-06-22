using CAPA.MODELO;
using CAPA.NEGOCIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAPA.WEB.Controllers
{
    public class CorreoController : AppController
    {
        CorreoBL correoBL;

        public CorreoController()
        {
            correoBL = new CorreoBL();
        }

        [HttpPost]
        public ActionResult SolicitarAyuda(Correo correo)
        {
            ResultadoWeb resultadoConsulta = new ResultadoWeb();
            #region Código programable
            resultadoConsulta = correoBL.SolicitarAyuda(correo);
            #endregion
            return JsonController(resultadoConsulta);
        }
    }
}