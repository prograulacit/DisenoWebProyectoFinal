using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/proveedor")]
    public class ProveedorController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Proveedor proveedor = new Proveedor();

            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT PROV_CODIGO, PROV_NOMBRE, PROV_DIRECCION, 
                                                            PROV_TELEFONO
                                                            FROM   PROVEEDOR WHERE PROV_CODIGO = @PROV_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@PROV_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        proveedor.PROV_CODIGO = sqlDataReader.GetInt32(0);
                        proveedor.PROV_NOMBRE = sqlDataReader.GetString(1);
                        proveedor.PROV_DIRECCION = sqlDataReader.GetString(2);
                        proveedor.PROV_TELEFONO = sqlDataReader.GetString(3);

                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(proveedor);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Proveedor> proveedores = new List<Proveedor>();

            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT PROV_CODIGO, PROV_NOMBRE, PROV_DIRECCION, 
                                                            PROV_TELEFONO
                                                            FROM   PROVEEDOR", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Proveedor proveedor = new Proveedor()
                        {
                            PROV_CODIGO = sqlDataReader.GetInt32(0),
                            PROV_NOMBRE = sqlDataReader.GetString(1),
                            PROV_DIRECCION = sqlDataReader.GetString(2),
                            PROV_TELEFONO = sqlDataReader.GetString(3),

                        };
                        proveedores.Add(proveedor);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(proveedores);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Proveedor proveedor)
        {
            if (proveedor == null)
                return BadRequest();

            if (RegistrarProveedor(proveedor))
                return Ok(proveedor);
            else
                return InternalServerError();

        }

        private bool RegistrarProveedor(Proveedor proveedor)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" INSERT INTO PROVEEDOR (PROV_NOMBRE, PROV_DIRECCION, 
                                                            PROV_TELEFONO) VALUES
                                                        (@PROV_NOMBRE, @PROV_DIRECCION, @PROV_TELEFONO)", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@PROV_NOMBRE", proveedor.PROV_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@PROV_DIRECCION", proveedor.PROV_DIRECCION);
                sqlCommand.Parameters.AddWithValue("@PROV_TELEFONO", proveedor.PROV_TELEFONO);


                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();
            }


            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Put(Proveedor proveedor)
        {
            if (proveedor == null)
                return BadRequest();

            if (ActualizarProveedor(proveedor))
                return Ok(proveedor);
            else
                return InternalServerError();

        }

        private bool ActualizarProveedor(Proveedor proveedor)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                  SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" UPDATE PROVEEDOR SET
                                                            PROV_NOMBRE = @PROV_NOMBRE,
                                                            PROV_DIRECCION = @PROV_DIRECCION,
                                                            PROV_TELEFONO = @PROV_TELEFONO
                                                          WHERE PROV_CODIGO = @PROV_CODIGO ", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@PROV_CODIGO", proveedor.PROV_CODIGO);
                sqlCommand.Parameters.AddWithValue("@PROV_NOMBRE", proveedor.PROV_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@PROV_DIRECCION", proveedor.PROV_DIRECCION);
                sqlCommand.Parameters.AddWithValue("@PROV_TELEFONO", proveedor.PROV_TELEFONO);

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

            if (EliminarProveedor(id))
                return Ok(id);
            else
                return InternalServerError();
        }

        private bool EliminarProveedor(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" DELETE PROVEEDOR
                                                          WHERE PROV_CODIGO = @PROV_CODIGO ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@PROV_CODIGO", id);
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
