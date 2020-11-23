using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/gerente")]
    public class GerenteController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Gerente gerente = new Gerente();

            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT GER_CODIGO, GER_NOMBRE, GER_APELLIDO1, 
                                                            GER_APELLIDO2, GER_DIRECCION, GER_TELEFONO
                                                            FROM   GERENTE WHERE GER_CODIGO = @GER_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@GER_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        gerente.GER_CODIGO = sqlDataReader.GetInt32(0);
                        gerente.GER_NOMBRE = sqlDataReader.GetString(1);
                        gerente.GER_APELLIDO1 = sqlDataReader.GetString(2);
                        gerente.GER_APELLIDO2 = sqlDataReader.GetString(3);
                        gerente.GER_DIRECCION = sqlDataReader.GetString(4);
                        gerente.GER_TELEFONO = sqlDataReader.GetString(5);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(gerente);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Gerente> gerentes = new List<Gerente>();

            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT GER_CODIGO, GER_NOMBRE, GER_APELLIDO1, 
                                                            GER_APELLIDO2, GER_DIRECCION, GER_TELEFONO
                                                            FROM   GERENTE", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Gerente gerente = new Gerente()
                        {
                            GER_CODIGO = sqlDataReader.GetInt32(0),
                            GER_NOMBRE = sqlDataReader.GetString(1),
                            GER_APELLIDO1 = sqlDataReader.GetString(2),
                            GER_APELLIDO2 = sqlDataReader.GetString(3),
                            GER_DIRECCION = sqlDataReader.GetString(4),
                            GER_TELEFONO = sqlDataReader.GetString(5)
                        };
                        gerentes.Add(gerente);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(gerentes);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Gerente gerente)
        {
            if (gerente == null)
                return BadRequest();

            if (RegistrarGerente(gerente))
                return Ok(gerente);
            else
                return InternalServerError();

        }

        private bool RegistrarGerente(Gerente gerente)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" INSERT INTO GERENTE (GER_NOMBRE, GER_APELLIDO1, 
                                                            GER_APELLIDO2, GER_DIRECCION, GER_TELEFONO) VALUES
                                                        (@GER_NOMBRE, @GER_APELLIDO1, @GER_APELLIDO2, @GER_DIRECCION, @GER_TELEFONO )", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@GER_NOMBRE", gerente.GER_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@GER_APELLIDO1", gerente.GER_APELLIDO1);
                sqlCommand.Parameters.AddWithValue("@GER_APELLIDO2", gerente.GER_APELLIDO2);
                sqlCommand.Parameters.AddWithValue("@GER_DIRECCION", gerente.GER_DIRECCION);
                sqlCommand.Parameters.AddWithValue("@GER_TELEFONO", gerente.GER_TELEFONO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();
            }


            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Put(Gerente gerente)
        {
            if (gerente == null)
                return BadRequest();

            if (ActualizarGerente(gerente))
                return Ok(gerente);
            else
                return InternalServerError();

        }

        private bool ActualizarGerente(Gerente gerente)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                  SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" UPDATE GERENTE SET
                                                            GER_NOMBRE = @GER_NOMBRE,
                                                            GER_APELLIDO1 = @GER_APELLIDO1,
                                                            GER_APELLIDO2 = @GER_APELLIDO2,
                                                            GER_DIRECCION = @GER_DIRECCION,
                                                            GER_TELEFONO = @GER_TELEFONO
                                                          WHERE GER_CODIGO = @GER_CODIGO ", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@GER_CODIGO", gerente.GER_CODIGO);
                sqlCommand.Parameters.AddWithValue("@GER_NOMBRE", gerente.GER_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@GER_APELLIDO1", gerente.GER_APELLIDO1);
                sqlCommand.Parameters.AddWithValue("@GER_APELLIDO2", gerente.GER_APELLIDO2);
                sqlCommand.Parameters.AddWithValue("@GER_DIRECCION", gerente.GER_DIRECCION);
                sqlCommand.Parameters.AddWithValue("@GER_TELEFONO", gerente.GER_TELEFONO);

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

            if (EliminarGerente(id))
                return Ok(id);
            else
                return InternalServerError();
        }

        private bool EliminarGerente(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" DELETE GERENTE
                                                          WHERE GER_CODIGO = @GER_CODIGO ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@GER_CODIGO", id);
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
