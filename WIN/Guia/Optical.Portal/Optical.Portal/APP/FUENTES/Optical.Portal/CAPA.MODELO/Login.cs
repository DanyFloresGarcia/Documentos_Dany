using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA.MODELO
{
    public class Login
    {
        [Required]
        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }
        public bool DebePermanecerConectado { get; set; }
    }
}
