using CAPA.MODELO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace CAPA.UTIL
{
    public static class Implementacion
    {
        public static void DeleteCookie(string key)
        {
            try
            {
                HttpCookie httpCookie = new HttpCookie(key);
                httpCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(httpCookie);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static HttpCookie GetCookie(string key)
        {
            try
            {
                return HttpContext.Current.Request.Cookies[key];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetCookie(string key, string value)
        {
            try
            {
                HttpContext.Current.Response.Cookies[key].Value = value;
                HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies[key].HttpOnly = true;
                //HttpContext.Current.Response.Cookies[key].Domain = GetConfigKey<string>("DominioEmpresa");
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public static void SetSession(string key, object value)
        {
            try
            {
                HttpContext.Current.Session[key] = value;
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public static T GetSession<T>(string key)
        {
            try
            {
                var value = (T)HttpContext.Current.Session[key];

                return value == null ? default(T) : value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T GetConfigKey<T>(string key)
        {
            try
            {
                string valor = ConfigurationManager.AppSettings[key].ToString();

                return valor == null ? default(T) : (T)Convert.ChangeType(valor, typeof(T));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UsuarioSesion DatosUsuarioSesion()
        {
            UsuarioSesion usuarioSesion = new UsuarioSesion();

            try
            {
                if (GetSession<UsuarioSesion>("UsuarioSesion") != null)
                {
                    usuarioSesion = GetSession<UsuarioSesion>("UsuarioSesion");
                }
                else
                {
                    usuarioSesion = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return usuarioSesion;
        }

        public static ResultadoWeb MensajeExcepcion(DatosExcepcionWeb datosExcepcionWeb)
        {
            string ambiente = GetConfigKey<string>("AmbienteProgramacion");
            ResultadoWeb resultadoWeb = new ResultadoWeb()
            {
                EstadoSolicitud = new EstadoSolicitud()
                {
                    EstaCorrecto = false,
                    TipoNotificacionId = 4
                }
            };
            Exception ex = datosExcepcionWeb.Exception;
            datosExcepcionWeb.MensajeDetalle = MensajeExcepcionDetalle(datosExcepcionWeb).EstadoSolicitud.MensajeRespuesta;

            DatosGuardadoArchivoWeb datosGuardadoArchivoWeb = new DatosGuardadoArchivoWeb()
            {
                Directorio = GetConfigKey<string>("DirectorioLogger"),
                Nombre = DateTime.Now.ToString("yyyy-MM-dd") + ".txt",
                TipoRutaId = 1,
                TipoGuardadoId = 2
            };

            if (ex.InnerException != null)
            {
                if (ex.InnerException.Message == "Operation returned an invalid status code 'Unauthorized'")
                {
                    resultadoWeb.EstadoSolicitud.TipoNotificacionId = 6;
                    resultadoWeb.EstadoSolicitud.MensajeRespuesta = "Sesión Expirada";
                }
                else
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        resultadoWeb.EstadoSolicitud.MensajeRespuesta = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        resultadoWeb.EstadoSolicitud.MensajeRespuesta = ex.InnerException.Message;
                    }

                    switch (ambiente)
                    {
                        case "Desarrollo":
                            resultadoWeb.EstadoSolicitud.MensajeRespuesta = $"{datosExcepcionWeb.MensajeDetalle} {resultadoWeb.EstadoSolicitud.MensajeRespuesta}";
                            break;
                        default:
                            //Guardar logger
                            datosGuardadoArchivoWeb.Contenido = $"{datosExcepcionWeb.MensajeDetalle} {resultadoWeb.EstadoSolicitud.MensajeRespuesta}";
                            CrearArchivo(datosGuardadoArchivoWeb);
                            //Mostrar un mensaje al usuario
                            resultadoWeb.EstadoSolicitud.MensajeRespuesta = "Ocurrió un error inesperado, Comuníquese con Mesa de Ayuda.\n(" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ")";
                            break;
                    }
                }
            }
            else
            {
                switch (ambiente)
                {
                    case "Desarrollo":
                        resultadoWeb.EstadoSolicitud.MensajeRespuesta = $"{datosExcepcionWeb.MensajeDetalle} {ex.Message}";
                        break;
                    default:
                        //Guardar logger
                        datosGuardadoArchivoWeb.Contenido = $"{datosExcepcionWeb.MensajeDetalle} {ex.Message}";
                        CrearArchivo(datosGuardadoArchivoWeb);
                        //Mostrar mensaje al usuario
                        resultadoWeb.EstadoSolicitud.MensajeRespuesta = "Ocurrió un error inesperado, Comuníquese con Mesa de Ayuda.\n(" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ")";
                        break;
                }
            }

            return resultadoWeb;
        }

        //Detalle de excepciones
        public static ResultadoWeb MensajeExcepcionDetalle(DatosExcepcionWeb datosExcepcionWeb)
        {
            try
            {
                Exception ex = datosExcepcionWeb.Exception;
                ResultadoWeb resultadoWeb = new ResultadoWeb();
                StackTrace stackTrace = new StackTrace(ex, true);
                string metodo = string.Empty;
                string capa = string.Empty;
                int lineaError = 0;
                string usuarioLogin = string.Empty;
                string nombreCompletoMetodo = string.Empty;

                if (datosExcepcionWeb.UsuarioSesionId == null)
                {
                    usuarioLogin = "sistema";
                }
                else
                {
                    usuarioLogin = datosExcepcionWeb.UsuarioSesionId.ToString();
                }

                foreach (var stackFrame in stackTrace.GetFrames())
                {
                    if (stackFrame.GetMethod().ReflectedType != null)
                    {
                        lineaError = stackFrame.GetFileLineNumber();

                        switch (datosExcepcionWeb.TipoExcepcionCapaId)
                        {
                            case 1:
                                //Func<Dictionary<string, List<string>>, ServicioWeb, ResultadoWeb> funcDao = (Func<Dictionary<string, List<string>>, ServicioWeb, ResultadoWeb>)datosExcepcionWeb.FuncDao;
                                nombreCompletoMetodo = datosExcepcionWeb.FuncDao.GetMethod(); 
                                break;
                            case 2:
                                Func<ResultadoWeb> funcBL = (Func<ResultadoWeb>)datosExcepcionWeb.FuncBL;
                                nombreCompletoMetodo = funcBL.Method.ReflectedType.FullName;
                                break;
                            default:
                                break;
                        }

                        if (lineaError > 0)
                        {
                            string[] nombreMetodoRef = stackFrame.GetMethod().ReflectedType.FullName.Split(new string[] { "+<>c" }, StringSplitOptions.None);
                            string[] nombreFuncion = nombreMetodoRef[0].Split('.');
                            metodo = nombreFuncion[nombreFuncion.Length - 1];

                            for (int i = 0; i < nombreFuncion.Length - 1; i++)
                            {
                                capa = capa + "." + nombreFuncion[i];
                            }
                            capa = capa.Substring(1, capa.Length - 1);
                            break;
                        }
                    }
                }

                string mensajeDetalle = $"Fecha: {DateTime.Now} || Usuario: {usuarioLogin} || Capa: {capa} || Método: {metodo} || Linea Error: {lineaError} || Mensaje Error:";

                resultadoWeb.EstadoSolicitud = new EstadoSolicitud()
                {
                    EstaCorrecto = true,
                    MensajeRespuesta = mensajeDetalle,
                    TipoNotificacionId = 4
                };

                return resultadoWeb;
            }
            catch (Exception excep)
            {
                throw excep;
            }
        }

        //Crear Archivo Local
        private static ResultadoWeb CrearArchivo(DatosGuardadoArchivoWeb datosGuardadoArchivoWeb)
        {
            try
            {
                ResultadoWeb resultadoConsultaWeb = new ResultadoWeb();

                if (!Directory.Exists(datosGuardadoArchivoWeb.Directorio))
                {
                    Directory.CreateDirectory(datosGuardadoArchivoWeb.Directorio);
                }

                switch (datosGuardadoArchivoWeb.TipoRutaId)
                {
                    //Local
                    case 1:
                        string directorioCompleto = datosGuardadoArchivoWeb.Directorio + datosGuardadoArchivoWeb.Nombre;
                        switch (datosGuardadoArchivoWeb.TipoGuardadoId)
                        {
                            //Reemplazar
                            case 1:
                                if (File.Exists(directorioCompleto))
                                {
                                    File.Delete(directorioCompleto);
                                }
                                using (FileStream fs = File.Create(directorioCompleto))
                                {
                                    using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                                    {
                                        writer.Write(datosGuardadoArchivoWeb.Contenido);
                                    }
                                }
                                break;
                            //Escribir sobre uno existente
                            case 2:
                                string contenido = string.Empty;

                                if (!File.Exists(directorioCompleto))
                                {
                                    File.Create(directorioCompleto).Close();
                                }
                                using (StreamWriter writer = File.AppendText(directorioCompleto))
                                {
                                    writer.WriteLine(datosGuardadoArchivoWeb.Contenido);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    //Ftp
                    case 2:
                        break;
                    default:
                        break;
                }

                return resultadoConsultaWeb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string CheckStr(object value)
        {
            string salida;

            if (value == null || value == System.DBNull.Value)
                salida = "";
            else
                salida = value.ToString();

            return salida.Trim();
        }

        public static void LimpiarSesion()
        {
            SetSession("CurrentUser", null);
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Session.Abandon();
            SetCookie("ASP_AUTHUSERS", null);
            SetCookie("ASP_AUTPWDSS", null);
            SetCookie("LoggedNombreUsuario", null);
            SetCookie("LoggedContrasena", null);
        }

        public static void SendEmail(Correo correo)
        {
            try
            {
                var smtp = new SmtpClient();
                var message = new MailMessage();

                smtp = new SmtpClient
                {
                    Host = "smtp.office365.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("notificaciones@optical.pe", "z6Wg$5h9pf"),
                    TargetName = "STARTTLS/smtp.office365.com"
                };

                message = new MailMessage("notificaciones@optical.pe", correo.CorreoReceptor);

                message.Subject = "Solicitud de Ayuda. Inicio de Sesión.";
                message.Body = correo.MensajeRemitente;

                var correoCopiaBd = new MailAddress(correo.CorreoRemitente);
                message.CC.Add(correoCopiaBd);

                foreach (var ArchivoAdjunto in correo.ListaDeArchivoAdjunto)
                {
                    message.Attachments.Add(new Attachment(ArchivoAdjunto.InputStream, Path.GetFileName(ArchivoAdjunto.FileName), ArchivoAdjunto.ContentType));
                }
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
