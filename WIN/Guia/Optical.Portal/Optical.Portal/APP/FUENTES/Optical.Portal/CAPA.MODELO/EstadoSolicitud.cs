using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA.MODELO
{
    public class EstadoSolicitud
    {
        public bool EstaCorrecto { get; set; }
        public string MensajeRespuesta { get; set; }
        public int TipoNotificacionId { get; set; }
    }
}
