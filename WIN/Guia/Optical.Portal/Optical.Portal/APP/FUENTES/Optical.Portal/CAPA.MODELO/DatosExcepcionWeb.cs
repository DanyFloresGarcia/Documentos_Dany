using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA.MODELO
{
    public class DatosExcepcionWeb
    {
        public Exception Exception { get; set; }
        public string MensajeDetalle { get; set; }
        public dynamic FuncDao { get; set; }
        public dynamic FuncBL { get; set; }
        public int TipoExcepcionCapaId { get; set; } //1 = Dao, 2 = BL
        public dynamic UsuarioSesionId { get; set; }
    }
}
