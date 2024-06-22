using CAPA.MODELO;
using CAPA.UTIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA.NEGOCIO
{
    public class CorreoBL: AppBL
    {
        public ResultadoWeb SolicitarAyuda(Correo correo)
        {
            return EjecutarBL(() =>
            {
                ResultadoWeb resultadoWeb = new ResultadoWeb();

                #region Codigo programable
                if (correo.ListaDeArchivoAdjunto == null || correo.ListaDeArchivoAdjunto.Count == 0)
                {
                    resultadoWeb.EstadoSolicitud = new EstadoSolicitud()
                    {
                        EstaCorrecto = false,
                        MensajeRespuesta = "Debe Subir un Archivo como Evidencia.",
                        TipoNotificacionId = 3
                    };
                }
                else if (correo.CorreoRemitente == null || correo.CorreoRemitente == string.Empty)
                {
                    resultadoWeb.EstadoSolicitud = new EstadoSolicitud()
                    {
                        EstaCorrecto = false,
                        MensajeRespuesta = "Ingrese su Correo.",
                        TipoNotificacionId = 3
                    };
                }
                else if (correo.MensajeRemitente == null || correo.MensajeRemitente == string.Empty)
                {
                    resultadoWeb.EstadoSolicitud = new EstadoSolicitud()
                    {
                        EstaCorrecto = false,
                        MensajeRespuesta = "Ingrese un Mensaje en su Solicitud.",
                        TipoNotificacionId = 3
                    };
                } else
                {
                    correo.CorreoReceptor = "mesadeayuda@optical.pe";

                    Implementacion.SendEmail(correo);
                    resultadoWeb.EstadoSolicitud = new EstadoSolicitud()
                    {
                        EstaCorrecto = true,
                        MensajeRespuesta = "Solicitud Enviada Correctamente.",
                        TipoNotificacionId = 1
                    };
                }
                #endregion

                return resultadoWeb;
            },
            #region Datos Ejecutar BL
            new DatosEjecutarBL()
            {
                DebeValidarSesion = false,
                UsuarioSesionId = Implementacion.GetSession<UsuarioSesion>("UsuarioSesion") == null ? "sistema" : Implementacion.GetSession<UsuarioSesion>("UsuarioSesion").NombreUsuario
            }
            #endregion
            );
        }
    }
}
