using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA.MODELO
{
    public class ServicioWeb
    {
        public string RutaWebApi { get; set; }
        public string RutaMetodoApi { get; set; }
        public Method Metodo { get; set; }
        public bool EsSincrono { get; set; }
        public dynamic Entidad { get; set; }
        public bool UsaToken { get; set; }
        public ServicioWeb()
        {
            EsSincrono = true;
        }
    }
}
