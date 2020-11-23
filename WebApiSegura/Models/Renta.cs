using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class Renta
    {
        public int REN_CODIGO { get; set; }
        public string USU_IDENTIFICACION { get; set; }
        public int AUTO_CODIGO { get; set; }
        public int EMP_CODIGO { get; set; }
        public string REN_DESCRIPCION { get; set; }
        public int REN_CANTIDAD { get; set; }
        public decimal REN_PRECIO { get; set; }
        public DateTime REN_FEC_RETIRO { get; set; }
        public DateTime REN_FEC_RETORNO { get; set; }
    }
}