using CAPA.CONEXION.Dao;
using CAPA.CONSUMO.Models;
using CAPA.MODELO;
using CAPA.UTIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA.NEGOCIO
{
    public class UsuarioBL: AppBL
    {
        UsuarioDao usuarioDao;

        public UsuarioBL()
        {
            usuarioDao = new UsuarioDao();
        }

        public ResultadoWeb DatosPorNombreUsuario(Login login)
        {
            return EjecutarBL(() =>
            {
                ResultadoWeb resultadoWeb = new ResultadoWeb();

                #region Codigo programable
                resultadoWeb = usuarioDao.DatosPorNombreUsuario(login);
                #endregion

                return resultadoWeb;
            },
            #region Datos Ejecutar BL
            new DatosEjecutarBL()
            {
                DebeValidarSesion = false,
                UsuarioSesionId = login.NombreUsuario
            }
            #endregion
            );
        }

        public ResultadoWeb DatosUsuarioSesion()
        {
            return EjecutarBL(() =>
            {
                ResultadoWeb resultadoWeb = new ResultadoWeb();

                #region Codigo programable
                if (Implementacion.GetSession<UsuarioSesion>("UsuarioSesion") == null)
                {
                    resultadoWeb.EstadoSolicitud = new MODELO.EstadoSolicitud()
                    {
                        EstaCorrecto = false,
                        MensajeRespuesta = "Sesión Expirada",
                        TipoNotificacionId = 6
                    };
                } else
                {
                    resultadoWeb.UsuarioSesion = Implementacion.GetSession<UsuarioSesion>("UsuarioSesion");
                    resultadoWeb.EstadoSolicitud = new MODELO.EstadoSolicitud()
                    {
                        EstaCorrecto = true,
                        MensajeRespuesta = "OK",
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

        public ResultadoWeb Verificar(Usuario usuario)
        {
            return EjecutarBL(() =>
            {
                ResultadoWeb resultadoWeb = new ResultadoWeb();

                #region Codigo programable
                resultadoWeb = usuarioDao.Verificar(usuario);
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
