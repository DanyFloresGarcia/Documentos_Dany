using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA.MODELO
{
    public class DatosGuardadoArchivoWeb
    {
        public string Nombre { get; set; }
        public string Directorio { get; set; }
        public string Contenido { get; set; }
        public int TipoRutaId { get; set; } //1 = local, 2 = ftp
        public int TipoGuardadoId { get; set; } //1 = reemplazar, 2 = agregar, 3 = crear uno nuevo
    }
}
