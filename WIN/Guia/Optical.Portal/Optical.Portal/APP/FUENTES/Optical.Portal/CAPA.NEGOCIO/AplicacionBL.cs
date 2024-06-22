using CAPA.CONEXION.Dao;
using CAPA.MODELO;
using CAPA.UTIL;

namespace CAPA.NEGOCIO
{
    public class AplicacionBL: AppBL
    {
        AplicacionDao aplicacionDao;

        public AplicacionBL()
        {
            aplicacionDao = new AplicacionDao();
        }

        public ResultadoWeb ListaPorNombreUsuario()
        {
            return EjecutarBL(() =>
            {
                ResultadoWeb resultadoWeb = new ResultadoWeb();

                #region Codigo programable
                UsuarioSesion usuarioSesion = Implementacion.GetSession<UsuarioSesion>("UsuarioSesion");
                resultadoWeb = aplicacionDao.ListaPorNombreUsuario(usuarioSesion);

                if (resultadoWeb.EstadoSolicitud.EstaCorrecto == true)
                {
                    int x = 0;
                    string cadena = $"<div class='row'>";

                    if (resultadoWeb.ListaDeAplicacion != null && resultadoWeb.ListaDeAplicacion.Count > 0)
                    {
                        foreach (var aplicacion in resultadoWeb.ListaDeAplicacion)
                        {
                            if (x == 3)
                            {
                                x = 0;
                                cadena = $"{cadena}</div><div class='row'>";
                            }

                            cadena = cadena + "<div class=\"col-4 p-t-30\">";
                            cadena = cadena + $"<div class=\"aplicacion\" id=\"aplicacion\" onclick=\"Aplicacion.AbrirApp('{aplicacion.Ruta}')\" style=\"background-color: {aplicacion.Color};\">";
                            cadena = cadena + "<div class=\"img-aplicacion\" id=\"img-aplicacion\">";
                            cadena = cadena + "<div class=\"row\">";
                            cadena = cadena + "<div class=\"col-2\">";
                            cadena = cadena + $"<h3><i class=\"{aplicacion.Icono}\"></i></h3>";
                            cadena = cadena + "</div>";
                            cadena = cadena + "<div class=\"col-10 text-left\">";
                            cadena = cadena + $"<h3>{aplicacion.Nombre}</h3>";
                            cadena = cadena + "</div>";
                            cadena = cadena + "</div>";
                            cadena = cadena + "</div>";
                            cadena = cadena + "<div class=\"text-aplicacion\" id =\"text-aplicacion\">";
                            cadena = cadena + $"<h4 class=\"text-det-aplicacion\">{aplicacion.Descripcion}</h4>";
                            cadena = cadena + "</div>";
                            cadena = cadena + "<div class=\"url-ver-aplicacion text-right\" id =\"url-ver-aplicacion\">";
                            cadena = cadena + "<label>Ir al Aplicativo</label> <i class=\"fas fa-arrow-right\"></i>";
                            cadena = cadena + "</div>";
                            cadena = cadena + "<div class=\"ico-ver-aplicacion\" id =\"ico-ver-aplicacion\">";
                            cadena = cadena + "<i class=\"fas fa-arrow-right\"></i>";
                            cadena = cadena + "</div>";
                            cadena = cadena + "</div>";
                            cadena = cadena + "</div>";

                            x = x + 1;
                        }

                        if (x < 3)
                        {
                            cadena = $"{cadena}</div>";
                        }

                        resultadoWeb.Cadena = cadena;
                    }
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
