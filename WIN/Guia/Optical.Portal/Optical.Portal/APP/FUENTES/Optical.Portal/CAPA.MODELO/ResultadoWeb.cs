using CAPA.CONSUMO.Models;
using System.Collections.Generic;

namespace CAPA.MODELO
{
    public class ResultadoWeb
    {
        public EstadoSolicitud EstadoSolicitud { get; set; }
        public dynamic Usuario { get; set; }
        public dynamic ListaDeVariableConfiguracion { get; set; }
        public List<Aplicacion> ListaDeAplicacion { get; set; }
        public string Cadena { get; set; }
        public UsuarioSesion UsuarioSesion { get; set; }
        public string RutaAplicativo { get; set; }
    }
}
