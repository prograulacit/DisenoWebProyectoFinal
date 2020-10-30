using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/combustible")]
    public class CombustibleController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Combustible combustible = new Combustible();
            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(
                        @"SELECT COMB_CODIGO, COMB_TIPO 
                        FROM COMBUSTIBLE 
                        WHERE COMB_CODIGO = @COMB_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@COMB_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        combustible.COMB_CODIGO = sqlDataReader.GetInt32(0);
                        combustible.COMB_TIPO = sqlDataReader.GetString(1);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(combustible);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Combustible> combustibles = new List<Combustible>();

            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(
                        @"SELECT COMB_CODIGO, COMB_TIPO
                        FROM COMBUSTIBLE", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Combustible combustible = new Combustible()
                        {
                            COMB_CODIGO= sqlDataReader.GetInt32(0),
                            COMB_TIPO = sqlDataReader.GetString(1)
                        };
                        combustibles.Add(combustible);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(combustibles);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Combustible combustible)
        {
            if (combustible == null)
                return BadRequest();

            if (RegistrarCombustible(combustible))
                return Ok(combustible);
            else
                return InternalServerError();

        }

        private bool RegistrarCombustible(Combustible combustible)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(
                    @"INSERT INTO COMBUSTIBLE (COMB_TIPO) 
                    VALUES(@COMB_TIPO)", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@COMB_TIPO", combustible.COMB_TIPO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();
            }
            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Put(Combustible combustible)
        {
            if (combustible == null)
                return BadRequest();

            if (ActualizarCombustible(combustible))
                return Ok(combustible);
            else
                return InternalServerError();

        }

        private bool ActualizarCombustible(Combustible combustible)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                  SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(
                    @"UPDATE COMBUSTIBLE SET
                    COMB_TIPO = @COMB_TIPO
                    WHERE COMB_CODIGO = @COMB_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@COMB_CODIGO", combustible.COMB_CODIGO);
                sqlCommand.Parameters.AddWithValue("@COMB_TIPO", combustible.COMB_TIPO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();
            }

            return resultado;
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            if (EliminarCombustible(id))
                return Ok(id);
            else
                return InternalServerError();
        }

        private bool EliminarCombustible(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"
                    DELETE COMBUSTIBLE
                    WHERE COMB_CODIGO = @COMB_CODIGO", sqlConnection);   

                sqlCommand.Parameters.AddWithValue("@COMB_CODIGO", id);
                sqlConnection.Open();
                int filasAfectadas = sqlCommand.ExecuteNonQuery();
                if (filasAfectadas > 0)
                    resultado = true;
                sqlConnection.Close();
            }
            return resultado;
        }
    }
}