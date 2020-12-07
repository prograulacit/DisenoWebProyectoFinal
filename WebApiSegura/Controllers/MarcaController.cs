using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/marca")]
    public class MarcaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Marca marca = new Marca();
            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(
                        @"SELECT MAR_CODIGO, MAR_NOMBRE
                        FROM MARCA
                        WHERE MAR_CODIGO = @MAR_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@MAR_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        marca.MAR_CODIGO = sqlDataReader.GetInt32(0);
                        marca.MAR_NOMBRE = sqlDataReader.GetString(1);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(marca);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Marca> marcas = new List<Marca>();

            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(
                        @"SELECT MAR_CODIGO, MAR_NOMBRE
                        FROM MARCA", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Marca marca = new Marca()
                        {
                            MAR_CODIGO = sqlDataReader.GetInt32(0),
                            MAR_NOMBRE = sqlDataReader.GetString(1)
                        };
                        marcas.Add(marca);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(marcas);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Marca marca)
        {
            if (marca == null)
                return BadRequest();

            if (RegistrarMarca(marca))
                return Ok(marca);
            else
                return InternalServerError();
        }

        private bool RegistrarMarca(Marca marca)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(
                    @"INSERT INTO MARCA (MAR_NOMBRE) 
                    VALUES(@MAR_NOMBRE)", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@MAR_NOMBRE", marca.MAR_NOMBRE);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();
            }
            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Put(Marca marca)
        {
            if (marca == null)
                return BadRequest();

            if (ActualizarMarca(marca))
                return Ok(marca);
            else
                return InternalServerError();

        }

        private bool ActualizarMarca(Marca marca)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                  SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(
                    @"UPDATE MARCA SET
                    MAR_NOMBRE = @MAR_NOMBRE
                    WHERE MAR_CODIGO = @MAR_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@MAR_CODIGO", marca.MAR_CODIGO);
                sqlCommand.Parameters.AddWithValue("@MAR_NOMBRE", marca.MAR_NOMBRE);

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

            if (EliminarMarca(id))
                return Ok(id);
            else
                return InternalServerError();
        }

        private bool EliminarMarca(int id)
        {
            try
            {
                bool resultado = false;

                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"
                    DELETE MARCA
                    WHERE MAR_CODIGO = @MAR_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@MAR_CODIGO", id);
                    sqlConnection.Open();
                    int filasAfectadas = sqlCommand.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                        resultado = true;
                    sqlConnection.Close();
                }
                return resultado;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}