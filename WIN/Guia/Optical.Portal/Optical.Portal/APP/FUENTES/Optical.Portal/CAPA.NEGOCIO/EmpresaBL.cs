using CAPA.CONEXION.Dao;
using CAPA.MODELO;
using CAPA.UTIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA.NEGOCIO
{
    public class EmpresaBL: AppBL
    {
        EmpresaDao empresaDao;

        public EmpresaBL()
        {
            empresaDao = new EmpresaDao();
        }

        public ResultadoWeb DefectoPorNombreUsuario()
        {
            return EjecutarBL(() =>
            {
                ResultadoWeb resultadoWeb = new ResultadoWeb();

                #region Codigo programable
                UsuarioSesion usuarioSesion = Implementacion.GetSession<UsuarioSesion>("UsuarioSesion");
                resultadoWeb = empresaDao.DefectoPorNombreUsuario(usuarioSesion);
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
