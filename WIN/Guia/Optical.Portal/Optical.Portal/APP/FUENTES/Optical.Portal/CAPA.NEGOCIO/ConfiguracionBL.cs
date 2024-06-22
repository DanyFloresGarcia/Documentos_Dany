using CAPA.CONEXION.Dao;
using CAPA.MODELO;
using CAPA.UTIL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA.NEGOCIO
{
    public class ConfiguracionBL: AppBL
    {
        ConfiguracionDao configuracionDao;

        public ConfiguracionBL()
        {
            configuracionDao = new ConfiguracionDao();
        }

        public ResultadoWeb VariableInternaListarPorAplicacionId()
        {
            return EjecutarBL(() => {
                var resultadoWeb = new ResultadoWeb();

                var variableConfiguracion = new VariableConfiguracion()
                {
                    AplicacionId = int.Parse(ConfigurationManager.AppSettings["AplicacionId"])
                };

                #region Codigo Programable
                //Aqui llamamos a la base de datos
                resultadoWeb = configuracionDao.VariableInternaListarPorAplicacionId(variableConfiguracion);

                //Lenamos las variables
                Implementacion.SetSession("VariableConfiguracionLista", resultadoWeb.ListaDeVariableConfiguracion);
                Implementacion.SetSession("AplicacionId", 0);

                resultadoWeb.EstadoSolicitud = new EstadoSolicitud()
                {
                    EstaCorrecto = true,
                    MensajeRespuesta = "Listado Correctamente"
                };
                #endregion

                return resultadoWeb;
            },
            #region Datos Ejecutar BL
            new DatosEjecutarBL()
            {
                DebeValidarSesion = false,
                UsuarioSesionId = null
            }
            #endregion
            );
        }
    }
}
