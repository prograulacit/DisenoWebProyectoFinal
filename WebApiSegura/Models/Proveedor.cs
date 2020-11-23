using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class Proveedor
    {
        public int PROV_CODIGO { get; set; }
        public string PROV_NOMBRE { get; set; }
        public string PROV_DIRECCION { get; set; }
        public string PROV_TELEFONO { get; set; }
    }
}