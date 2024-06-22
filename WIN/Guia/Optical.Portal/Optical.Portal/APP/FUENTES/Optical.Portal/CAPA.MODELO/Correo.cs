using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CAPA.MODELO
{
    public class Correo
    {
        public string CorreoReceptor { get; set; }
        public string CorreoRemitente { get; set; }
        public string MensajeRemitente { get; set; }
        public List<HttpPostedFileBase> ListaDeArchivoAdjunto { get; set; }
    }
}
