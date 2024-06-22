using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA.MODELO
{
    public class VariableConfiguracion
    {
        public int? AplicacionId { get; set; }
        public int AplicacionVariableId { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string IpUser { get; set; }
    }
}
