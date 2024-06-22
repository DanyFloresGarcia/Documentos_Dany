using CAPA.MODELO;
using CAPA.UTIL;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CAPA.CONEXION.Dao
{
    public class ConfiguracionDao : AppDao
    {
        public ResultadoWeb VariableInternaListarPorAplicacionId(VariableConfiguracion variableConfiguracion)
        {
            #region Codigo Programable
            List<VariableConfiguracion> listaVariableConfiguracion = new List<VariableConfiguracion>()
            {
                new VariableConfiguracion()
                {
                    Nombre = "AmbienteProgramacion",
                    Valor = Implementacion.GetConfigKey<string>("AmbienteProgramacion")
                },
                new VariableConfiguracion()
                {
                    Nombre = "DerechosAutor",
                    Valor = Implementacion.GetConfigKey<string>("DerechosAutor")
                },
                new VariableConfiguracion()
                {
                    Nombre = "VersionSistema",
                    Valor = Implementacion.GetConfigKey<string>("VersionSistema")
                },
                new VariableConfiguracion()
                {
                    Nombre = "NombreAplicacion",
                    Valor = Implementacion.GetConfigKey<string>("NombreAplicacion")
                },
                new VariableConfiguracion()
                {
                    Nombre = "Servidor",
                    Valor = Implementacion.GetConfigKey<string>("Servidor")
                }
            };

            ResultadoWeb resultadoWeb = new ResultadoWeb()
            {
                ListaDeVariableConfiguracion = listaVariableConfiguracion
            };

            return resultadoWeb;
            #endregion
        }
    }
}
