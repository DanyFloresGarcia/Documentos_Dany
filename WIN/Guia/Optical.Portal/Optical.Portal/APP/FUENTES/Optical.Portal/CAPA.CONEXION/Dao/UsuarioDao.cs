using CAPA.CONSUMO.Models;
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
    public class UsuarioDao: AppDao
    {
        public ResultadoWeb DatosPorNombreUsuario(Login login)
        {
            ResultadoWeb resultadoWeb = new ResultadoWeb();

            #region Código Programable
            resultadoWeb = EjecutarDao(new ServicioWeb()
            {
                Entidad = login,
                EsSincrono = true,
                UsaToken = false,
                RutaWebApi = Implementacion.GetConfigKey<string>("RutaWebServiceAuditoria"),
                RutaMetodoApi = "Usuario/DatosPorNombreUsuario",
                Metodo = Method.POST
            });
            #endregion

            return resultadoWeb;
        }

        public ResultadoWeb Verificar(Usuario usuario)
        {
            ResultadoWeb resultadoWeb = new ResultadoWeb();

            #region Código Programable
            resultadoWeb = EjecutarDao(new ServicioWeb()
            {
                Entidad = usuario,
                EsSincrono = true,
                UsaToken = false,
                RutaWebApi = Implementacion.GetConfigKey<string>("RutaWebServiceAuditoria"),
                RutaMetodoApi = "Usuario/Verificar",
                Metodo = Method.POST
            });
            #endregion

            return resultadoWeb;
        }
    }
}
