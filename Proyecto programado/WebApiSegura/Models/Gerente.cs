using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class Gerente
    {
        public int GER_CODIGO { get; set; }
        public string GER_NOMBRE { get; set; }
        public string GER_APELLIDO1 { get; set; }
        public string GER_APELLIDO2 { get; set; }
        public string GER_DIRECCION { get; set; }
        public string GER_TELEFONO { get; set; }
    }
}