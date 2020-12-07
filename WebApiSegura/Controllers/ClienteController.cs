using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Configuration;
using System.Data.SqlClient;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/cliente")]
    public class ClienteController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(string id)
        {
            Cliente cliente = new Cliente();

            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager
                   .ConnectionStrings["Reservas"].ConnectionString))
                {
                    SqlCommand sqlCommand =
                        new SqlCommand(
                            @"SELECT 
                            USU_IDENTIFICACION
                            , CLI_NOMBRE
                            , CLI_APELLIDO1
                            , CLI_APELLIDO2
                            , CLI_DIRECCION
                            , CLI_TELEFONO
                            FROM CLIENTE
                            WHERE USU_IDENTIFICACION = @USU_IDENTIFICACION;", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@USU_IDENTIFICACION", id);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        cliente.USU_IDENTIFICACION = sqlDataReader.GetString(0);
                        cliente.CLI_NOMBRE = sqlDataReader.GetString(1);
                        cliente.CLI_APELLIDO1 = sqlDataReader.GetString(2);
                        cliente.CLI_APELLIDO2 = sqlDataReader.GetString(3);
                        cliente.CLI_DIRECCION = sqlDataReader.GetString(4);
                        cliente.CLI_TELEFONO = sqlDataReader.GetString(5);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(cliente);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Cliente> reservas = new List<Cliente>();
            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager
                   .ConnectionStrings["Reservas"].ConnectionString))
                {
                    SqlCommand sqlCommand =
                        new SqlCommand(
                            @"SELECT 
                            USU_IDENTIFICACION
                            , CLI_NOMBRE
                            , CLI_APELLIDO1
                            , CLI_APELLIDO2
                            , CLI_DIRECCION
                            , CLI_TELEFONO
                            FROM CLIENTE;", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Cliente cliente = new Cliente()
                        {
                            USU_IDENTIFICACION = sqlDataReader.GetString(0),
                            CLI_NOMBRE = sqlDataReader.GetString(1),
                            CLI_APELLIDO1 = sqlDataReader.GetString(2),
                            CLI_APELLIDO2 = sqlDataReader.GetString(3),
                            CLI_DIRECCION = sqlDataReader.GetString(4),
                            CLI_TELEFONO = sqlDataReader.GetString(5),
                        };
                        reservas.Add(cliente);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(reservas);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Cliente cliente)
        {
            if (cliente == null)
                return BadRequest();

            if (registrarCliente(cliente))
                return Ok(cliente);
            else
                return InternalServerError();

        }

        private bool registrarCliente(Cliente cliente)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager
                   .ConnectionStrings["Reservas"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(
                    @"INSERT INTO CLIENTE (
                            USU_IDENTIFICACION
                            , CLI_NOMBRE
                            , CLI_APELLIDO1
                            , CLI_APELLIDO2
                            , CLI_DIRECCION
                            , CLI_TELEFONO)
                    VALUES(
                            @USU_IDENTIFICACION
                            , @CLI_NOMBRE
                            , @CLI_APELLIDO1
                            , @CLI_APELLIDO2
                            , @CLI_DIRECCION
                            , @CLI_TELEFONO)", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@USU_IDENTIFICACION", cliente.USU_IDENTIFICACION);
                sqlCommand.Parameters.AddWithValue("@CLI_NOMBRE", cliente.CLI_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@CLI_APELLIDO1", cliente.CLI_APELLIDO1);
                sqlCommand.Parameters.AddWithValue("@CLI_APELLIDO2", cliente.CLI_APELLIDO2);
                sqlCommand.Parameters.AddWithValue("@CLI_DIRECCION", cliente.CLI_DIRECCION);
                sqlCommand.Parameters.AddWithValue("@CLI_TELEFONO", cliente.CLI_TELEFONO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();
            }
            return resultado;
        }

        public IHttpActionResult Put(Cliente cliente)
        {
            if (cliente == null)
                return BadRequest();

            if (actualizarCliente(cliente))
                return Ok(cliente);
            else
                return InternalServerError();

        }

        private bool actualizarCliente(Cliente cliente)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                  SqlConnection(ConfigurationManager
                  .ConnectionStrings["Reservas"].ConnectionString))
            {
                SqlCommand sqlCommand =
                    new SqlCommand(
                        @"UPDATE CLIENTE SET
                            USU_IDENTIFICACION = @USU_IDENTIFICACION,
                            CLI_NOMBRE = @CLI_NOMBRE,
                            CLI_APELLIDO1 = @CLI_APELLIDO1,
                            CLI_APELLIDO2 = @CLI_APELLIDO2,
                            CLI_DIRECCION = @CLI_DIRECCION,
                            CLI_TELEFONO = @CLI_TELEFONO
                        WHERE USU_IDENTIFICACION = @USU_IDENTIFICACION;", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@USU_IDENTIFICACION", cliente.USU_IDENTIFICACION);
                sqlCommand.Parameters.AddWithValue("@CLI_NOMBRE", cliente.CLI_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@CLI_APELLIDO1", cliente.CLI_APELLIDO1);
                sqlCommand.Parameters.AddWithValue("@CLI_APELLIDO2", cliente.CLI_APELLIDO2);
                sqlCommand.Parameters.AddWithValue("@CLI_DIRECCION", cliente.CLI_DIRECCION);
                sqlCommand.Parameters.AddWithValue("@CLI_TELEFONO", cliente.CLI_TELEFONO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();
            }

            return resultado;
        }

        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            if (id == null)
                return BadRequest();

            if (eliminarCliente(id))
                return Ok(id);
            else
                return InternalServerError();
        }

        private bool eliminarCliente(string id)
        {
            try
            {
                bool resultado = false;

                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
                {
                    SqlCommand sqlCommand =
                        new SqlCommand(
                            @" DELETE CLIENTE
                        WHERE 
                        USU_IDENTIFICACION = @USU_IDENTIFICACION;", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@USU_IDENTIFICACION", id);
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
