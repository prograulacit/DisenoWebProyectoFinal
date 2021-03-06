﻿using System;

namespace WebApiSegura.Models
{
    public class Usuario
    {
        public int USU_CODIGO { get; set; }
        public string USU_IDENTIFICACION { get; set; }
        public string USU_NOMBRE { get; set; }
        public string USU_PASSWORD { get; set; }
        public string USU_EMAIL { get; set; }
        public string USU_ESTADO { get; set; }
        public DateTime USU_FECHA_NACIMIENTO { get; set; }
        public string USU_TELEFONO { get; set; }
        public string CadenaToken { get; set; }
    }
}