using CAPA.MODELO;
using CAPA.UTIL;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CAPA.CONEXION.Dao
{
    public class AppDao
    {
        public ResultadoWeb EjecutarDao(ServicioWeb servicioWeb)
        {
            ResultadoWeb resultadoWeb = new ResultadoWeb();

            try
            {
                var client = new RestClient(servicioWeb.RutaWebApi);
                var request = new RestRequest(servicioWeb.RutaMetodoApi, servicioWeb.Metodo);
                // adds to POST or URL querystring based on Method
                Dictionary<string, string> myDict = new Dictionary<string, string>();
                Type t = servicioWeb.Entidad.GetType();
                foreach (PropertyInfo pi in t.GetProperties())
                {
                    var valor = servicioWeb.Entidad.GetType().GetProperty(pi.Name).GetValue(servicioWeb.Entidad, null);
                    if (valor == null)
                    {
                        switch (pi.PropertyType.Name)
                        {
                            case "String":
                                valor = string.Empty;
                                break;
                            case "Nullable`1":
                                valor = 0;
                                break;
                            default:
                                break;
                        }
                    }
                    request.AddParameter(pi.Name, valor);
                }
                // execute the request
                if (servicioWeb.EsSincrono)
                {
                    var response = client.Execute<ResultadoWeb>(request);

                    if (response.ResponseStatus == ResponseStatus.Error)
                    {
                        resultadoWeb.EstadoSolicitud = new EstadoSolicitud()
                        {
                            EstaCorrecto = false,
                            MensajeRespuesta = response.ErrorMessage,
                            TipoNotificacionId = 4
                        };
                    } else
                    {
                        resultadoWeb = response.Data;
                    }
                }
                else
                {
                    var asyncHandle = client.ExecuteAsync<ResultadoWeb>(request, response => {
                        resultadoWeb = response.Data;
                    });
                }
            }
            catch (Exception ex)
            {
                DatosExcepcionWeb datosExcepcion = new DatosExcepcionWeb()
                {
                    Exception = ex,
                    FuncDao = new StackTrace().GetFrame(0),
                    TipoExcepcionCapaId = 1,
                    UsuarioSesionId = servicioWeb.Entidad.UsuarioSesionId
                };
                resultadoWeb = Implementacion.MensajeExcepcion(datosExcepcion);
            }

            return resultadoWeb;
        }
    }
}
