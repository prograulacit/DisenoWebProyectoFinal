using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class Cliente
    {
        public string USU_IDENTIFICACION { get; set; }
        public string CLI_NOMBRE { get; set; }
        public string CLI_APELLIDO1 { get; set; }
        public string CLI_APELLIDO2 { get; set; }
        public string CLI_DIRECCION { get; set; }
        public string CLI_TELEFONO { get; set; }
    }
}