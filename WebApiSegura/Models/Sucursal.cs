using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class Sucursal
    {
        public int SUC_CODIGO { get; set; }
        public int PROV_CODIGO { get; set; }
        public int GER_CODIGO { get; set; }
        public string SUC_NOMBRE { get; set; }
        public string SUC_DIRECCION { get; set; }
        public string SUC_TELEFONO { get; set; }
    }
}