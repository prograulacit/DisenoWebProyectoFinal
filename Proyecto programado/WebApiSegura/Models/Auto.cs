using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class Auto
    {
        public int AUTO_CODIGO { get; set; }
        public int MAR_CODIGO { get; set; }
        public int MOD_CODIGO { get; set; }
        public int COMB_CODIGO { get; set; }
        public int SUC_CODIGO { get; set; }
        public string AUTO_CANTIDAD { get; set; }
        public decimal AUTO_PRECIO { get; set; }
    }
}