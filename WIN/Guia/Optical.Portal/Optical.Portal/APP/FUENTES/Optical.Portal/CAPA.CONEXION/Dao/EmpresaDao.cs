using CAPA.MODELO;
using CAPA.UTIL;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA.CONEXION.Dao
{
    public class EmpresaDao: AppDao
    {
        public ResultadoWeb DefectoPorNombreUsuario(UsuarioSesion usuarioSesion)
        {
            ResultadoWeb resultadoWeb = new ResultadoWeb();

            #region Código Programable
            resultadoWeb = EjecutarDao(new ServicioWeb()
            {
                Entidad = usuarioSesion,
                EsSincrono = true,
                UsaToken = false,
                RutaWebApi = Implementacion.GetConfigKey<string>("RutaWebServiceAuditoria"),
                RutaMetodoApi = "Empresa/DefectoPorNombreUsuario",
                Metodo = Method.POST
            });
            #endregion

            return resultadoWeb;
        }
    }
}
