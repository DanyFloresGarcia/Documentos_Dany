using CAPA.MODELO;
using CAPA.UTIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA.NEGOCIO
{
    public class AppBL
    {
        public ResultadoWeb EjecutarBL(Func<ResultadoWeb> func, DatosEjecutarBL datosEjecutarBL)
        {
            ResultadoWeb resultadoWeb;

            try
            {
                if (datosEjecutarBL.DebeValidarSesion == true)
                {
                    UsuarioSesion usuarioSesion = Implementacion.DatosUsuarioSesion();

                    if (usuarioSesion == null)
                    {
                        resultadoWeb = new ResultadoWeb()
                        {
                            EstadoSolicitud = new EstadoSolicitud()
                            {
                                EstaCorrecto = false,
                                MensajeRespuesta = "Sesión expirada",
                                TipoNotificacionId = 6
                            }
                        };
                    }
                    else
                    {
                        resultadoWeb = func();
                    }
                }
                else
                {
                    resultadoWeb = func();
                }
            }
            catch (Exception ex)
            {
                DatosExcepcionWeb datosExcepcion = new DatosExcepcionWeb()
                {
                    Exception = ex,
                    FuncBL = func,
                    TipoExcepcionCapaId = 2,
                    UsuarioSesionId = datosEjecutarBL.UsuarioSesionId
                };
                resultadoWeb = Implementacion.MensajeExcepcion(datosExcepcion);
            }

            return resultadoWeb;
        }


        public ResultadoWeb Variables(AplicacionConfiguracion aplicacionConfiguracion)
        {
            return EjecutarBL(() =>
            {
                ResultadoWeb resultadoWeb = new ResultadoWeb();
                ConfiguracionBL configuracionBL = new ConfiguracionBL();
                List<VariableConfiguracion> variableConfiguracionLista = new List<VariableConfiguracion>();

                #region Codigo programable
                resultadoWeb = configuracionBL.VariableInternaListarPorAplicacionId();
                List<VariableConfiguracion> listaVariableConfiguracion = resultadoWeb.ListaDeVariableConfiguracion;

                string RutaUrlLogo = string.Empty;
                string RutaUrlFondoLogin = string.Empty;
                string RutaUrlLogoHeader = string.Empty;

                if (aplicacionConfiguracion.NombreDominio.Contains("win"))
                {
                    RutaUrlLogo = Implementacion.GetConfigKey<string>("RutaUrlLogoWin");
                    RutaUrlFondoLogin = Implementacion.GetConfigKey<string>("RutaUrlFondoLoginWin");
                    RutaUrlLogoHeader = Implementacion.GetConfigKey<string>("RutaUrlLogoHeaderWin");

                } else
                {
                    RutaUrlLogo = Implementacion.GetConfigKey<string>("RutaUrlLogoOptical");
                    RutaUrlFondoLogin = Implementacion.GetConfigKey<string>("RutaUrlFondoLoginOptical");
                    RutaUrlLogoHeader = Implementacion.GetConfigKey<string>("RutaUrlLogoHeaderOptical");
                }

                VariableConfiguracion variableConfiguracion = new VariableConfiguracion()
                {
                    Nombre = "RutaUrlLogo",
                    Valor = RutaUrlLogo
                };
                listaVariableConfiguracion.Add(variableConfiguracion);

                variableConfiguracion = new VariableConfiguracion()
                {
                    Nombre = "RutaUrlFondoLogin",
                    Valor = RutaUrlFondoLogin
                };
                listaVariableConfiguracion.Add(variableConfiguracion);

                variableConfiguracion = new VariableConfiguracion()
                {
                    Nombre = "RutaUrlLogoHeader",
                    Valor = RutaUrlLogoHeader
                };
                listaVariableConfiguracion.Add(variableConfiguracion);


                resultadoWeb.ListaDeVariableConfiguracion = listaVariableConfiguracion;
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

        public ResultadoWeb ViewController()
        {
            return EjecutarBL(() =>
            {
                ResultadoWeb resultadoWeb = new ResultadoWeb();
                #region Codigo programable
                LoginBL loginBL = new LoginBL();
                if (Implementacion.GetCookie("LoggedNombreUsuario") != null && Implementacion.GetCookie("LoggedNombreUsuario").Value != "")
                {
                    resultadoWeb = loginBL.Autenticar(new Login()
                    {
                        NombreUsuario = Cryp.Decrypt(Implementacion.GetCookie("LoggedNombreUsuario").Value),
                        Contrasena = Cryp.Decrypt(Implementacion.GetCookie("LoggedContrasena").Value),
                        DebePermanecerConectado = true
                    });
                }

                if (Implementacion.GetSession<UsuarioSesion>("UsuarioSesion") == null)
                {
                    resultadoWeb.EstadoSolicitud = new EstadoSolicitud()
                    {
                        EstaCorrecto = false,
                        MensajeRespuesta = "Sesión Expirada",
                        TipoNotificacionId = 6
                    };
                } else
                {
                    resultadoWeb.EstadoSolicitud = new EstadoSolicitud()
                    {
                        EstaCorrecto = true,
                        MensajeRespuesta = "Ok",
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
                UsuarioSesionId = null
            }
            #endregion
            );
        }
    }
}
