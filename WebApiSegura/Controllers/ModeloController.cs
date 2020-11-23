using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/modelo")]
    public class ModeloController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Modelo modelo = new Modelo();
            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(
                        @"SELECT MOD_CODIGO, MOD_NOMBRE, MOD_COLOR
                        FROM MODELO
                        WHERE MOD_CODIGO = @MOD_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@MOD_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        modelo.MOD_CODIGO = sqlDataReader.GetInt32(0);
                        modelo.MOD_NOMBRE = sqlDataReader.GetString(1);
                        modelo.MOD_COLOR = sqlDataReader.GetString(2);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(modelo);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Modelo> modelos = new List<Modelo>();

            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(
                        @"SELECT MOD_CODIGO, MOD_NOMBRE, MOD_COLOR
                        FROM MODELO", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Modelo modelo = new Modelo()
                        {
                            MOD_CODIGO = sqlDataReader.GetInt32(0),
                            MOD_NOMBRE = sqlDataReader.GetString(1),
                            MOD_COLOR= sqlDataReader.GetString(2)
                        };
                        modelos.Add(modelo);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(modelos);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Modelo modelo)
        {
            if (modelo == null)
                return BadRequest();

            if (RegistrarModelo(modelo))
                return Ok(modelo);
            else
                return InternalServerError();
        }

        private bool RegistrarModelo(Modelo modelo)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(
                    @"INSERT INTO MODELO (MOD_NOMBRE, MOD_COLOR) 
                    VALUES(@MOD_NOMBRE, @MOD_COLOR)", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@MOD_NOMBRE", modelo.MOD_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@MOD_COLOR", modelo.MOD_COLOR);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();
            }
            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Put(Modelo modelo)
        {
            if (modelo == null)
                return BadRequest();

            if (ActualizarModelo(modelo))
                return Ok(modelo);
            else
                return InternalServerError();
        }

        private bool ActualizarModelo(Modelo modelo)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                  SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(
                    @"UPDATE MODELO SET
                    MOD_NOMBRE = @MOD_NOMBRE,
                    MOD_COLOR = @MOD_COLOR
                    WHERE MOD_CODIGO = @MOD_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@MOD_CODIGO", modelo.MOD_CODIGO);
                sqlCommand.Parameters.AddWithValue("@MOD_NOMBRE", modelo.MOD_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@MOD_COLOR", modelo.MOD_COLOR);

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

            if (EliminarModelo(id))
                return Ok(id);
            else
                return InternalServerError();
        }

        private bool EliminarModelo(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"
                    DELETE MODELO
                    WHERE MOD_CODIGO = @MOD_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@MOD_CODIGO", id);
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
