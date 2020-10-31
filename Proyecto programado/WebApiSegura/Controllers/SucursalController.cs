using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    public class SucursalController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Sucursal sucursal = new Sucursal();

            try
            {
                using (SqlConnection sqlConncetion = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand =
                        new SqlCommand(@"SELECT SUC_CODIGO, PROV_CODIGO, GER_CODIGO, SUC_NOMBRE,
                                        SUC_DIRECCION, SUC_TELEFONO
                                        FROM   SUCURSAL WHERE SUC_CODIGO = @SUC_CODIGO", sqlConncetion);

                    sqlCommand.Parameters.AddWithValue("@SUC_CODIGO", id);

                    sqlConncetion.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        sucursal.SUC_CODIGO = sqlDataReader.GetInt32(0);
                        sucursal.PROV_CODIGO = sqlDataReader.GetInt32(1);
                        sucursal.GER_CODIGO = sqlDataReader.GetInt32(2);
                        sucursal.SUC_NOMBRE = sqlDataReader.GetString(3);
                        sucursal.SUC_DIRECCION = sqlDataReader.GetString(4);
                        sucursal.SUC_TELEFONO = sqlDataReader.GetString(5);
                    }

                    sqlConncetion.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(sucursal);
        }

        public IHttpActionResult GetAll()
        {
            List<Sucursal> sucursales = new List<Sucursal>();

            try
            {
                using (SqlConnection sqlConncetion = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand =
                        new SqlCommand(@"SELECT SUC_CODIGO, PROV_CODIGO, GER_CODIGO, SUC_NOMBRE,
                                        SUC_DIRECCION, SUC_TELEFONO
                                        FROM   SUCURSAL", sqlConncetion);

                    sqlConncetion.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Sucursal sucursal = new Sucursal()
                        {
                            SUC_CODIGO = sqlDataReader.GetInt32(0),
                            PROV_CODIGO = sqlDataReader.GetInt32(1),
                            GER_CODIGO = sqlDataReader.GetInt32(2),
                            SUC_NOMBRE = sqlDataReader.GetString(3),
                            SUC_DIRECCION = sqlDataReader.GetString(4),
                            SUC_TELEFONO = sqlDataReader.GetString(5)
                        };
                        sucursales.Add(sucursal);
                    }

                    sqlConncetion.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(sucursales);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Sucursal sucursal)
        {
            if (sucursal == null)
                return BadRequest();

            if (RegistrarSucursal(sucursal))
                return Ok();
            else
                return InternalServerError();
        }

        private bool RegistrarSucursal(Sucursal sucursal)
        {
            bool resultado = false;

            try
            {
                using (SqlConnection sqlConnection = new
                        SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT SUC_CODIGO, PROV_CODIGO, GER_CODIGO, SUC_NOMBRE,
                                        SUC_DIRECCION, SUC_TELEFONO
                                        FROM   SUCURSAL WHERE SUC_CODIGO = @SUC_CODIGO", sqlConnection);


                    sqlCommand.Parameters.AddWithValue("@PROV_CODIGO", sucursal.PROV_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@GER_CODIGO", sucursal.GER_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@SUC_NOMBRE", sucursal.SUC_NOMBRE);
                    sqlCommand.Parameters.AddWithValue("@SUC_DIRECCION", sucursal.SUC_DIRECCION);
                    sqlCommand.Parameters.AddWithValue("@SUC_TELEFONO", sucursal.SUC_TELEFONO);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                        resultado = true;

                    sqlConnection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Put(Sucursal sucursal)
        {
            if (sucursal == null)
                return BadRequest();

            if (ActualizarSucursal(sucursal))
                return Ok(sucursal);
            else
                return InternalServerError();
        }

        private bool ActualizarSucursal(Sucursal sucursal)
        {
            bool resultado = false;

            try
            {
                using (SqlConnection sqlConnection = new
                        SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"UPDATE SUCURSAL SET
                                                                PROV_CODIGO = @PROV_CODIGO
                                                                GER_CODIGO = @GER_CODIGO
                                                                SUC_NOMBRE = @SUC_NOMBRE
                                                                SUC_DIRECCION = @SUC_DIRECCION
                                                                SUC_TELEFONO = @SUC_TELEFONO
                                                                WHERE SUC_CODIGO = @SUC_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@SUC_CODIGO", sucursal.SUC_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@PROV_CODIGO", sucursal.PROV_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@GER_CODIGO", sucursal.GER_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@SUC_NOMBRE", sucursal.SUC_NOMBRE);
                    sqlCommand.Parameters.AddWithValue("@SUC_DIRECCION", sucursal.SUC_DIRECCION);
                    sqlCommand.Parameters.AddWithValue("@SUC_TELEFONO", sucursal.SUC_TELEFONO);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                        resultado = true;

                    sqlConnection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return resultado;
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (id > 0)
                return BadRequest();

            if (EliminarSucursal(id))
                return Ok(id);
            else
                return InternalServerError();

        }

        private bool EliminarSucursal(int id)
        {
            bool resultado = false;

            try
            {
                using (SqlConnection sqlConnection = new
                        SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"DELETE SUCURSAL
                                                                WHERE SUC_CODIGO = @SUC_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@SUC_CODIGO", id);
                    sqlConnection.Open();
                    int filasAfectadas = sqlCommand.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                        resultado = true;
                    sqlConnection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return resultado;
        }
    }
}
