using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA.MODELO
{
    public class UsuarioSesion
    {
        public int? UsuarioSesionId { get; set; }
        public string NombreUsuario { get; set; }
        public int? PersonaId { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
    }
}
