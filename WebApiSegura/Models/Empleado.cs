using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class Empleado
    {
        public int EMP_CODIGO { get; set; }
        public string EMP_NOMBRE { get; set; }
        public string EMP_APELLIDO1 { get; set; }
        public string EMP_APELLIDO2 { get; set; }
        public string EMP_DIRECCION { get; set; }
        public string EMP_TELEFONO { get; set; }
    }
}