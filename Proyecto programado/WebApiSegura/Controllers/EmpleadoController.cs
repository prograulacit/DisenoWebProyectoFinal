using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/empleado")]
    public class EmpleadoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Empleado empleado = new Empleado();

            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT EMP_CODIGO, EMP_NOMBRE, EMP_APELLIDO1, 
                                                            EMP_APELLIDO2, EMP_DIRECCION, EMP_TELEFONO
                                                            FROM   EMPLEADO WHERE EMP_CODIGO = @EMP_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@EMP_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        empleado.EMP_CODIGO = sqlDataReader.GetInt32(0);
                        empleado.EMP_NOMBRE = sqlDataReader.GetString(1);
                        empleado.EMP_APELLIDO1 = sqlDataReader.GetString(2);
                        empleado.EMP_APELLIDO2 = sqlDataReader.GetString(3);
                        empleado.EMP_DIRECCION = sqlDataReader.GetString(4);
                        empleado.EMP_TELEFONO = sqlDataReader.GetString(5);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(empleado);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Empleado> empleados = new List<Empleado>();

            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT EMP_CODIGO, EMP_NOMBRE, EMP_APELLIDO1, 
                                                            EMP_APELLIDO2, EMP_DIRECCION, EMP_TELEFONO
                                                            FROM   EMPLEADO", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Empleado empleado = new Empleado()
                        {
                            EMP_CODIGO = sqlDataReader.GetInt32(0),
                            EMP_NOMBRE = sqlDataReader.GetString(1),
                            EMP_APELLIDO1 = sqlDataReader.GetString(2),
                            EMP_APELLIDO2 = sqlDataReader.GetString(3),
                            EMP_DIRECCION = sqlDataReader.GetString(4),
                            EMP_TELEFONO = sqlDataReader.GetString(5)
                        };
                        empleados.Add(empleado);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(empleados);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Empleado empleado)
        {
            if (empleado == null)
                return BadRequest();

            if (RegistrarEmpleado(empleado))
                return Ok(empleado);
            else
                return InternalServerError();

        }

        private bool RegistrarEmpleado(Empleado empleado)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" INSERT INTO EMPLEADO (EMP_NOMBRE, EMP_APELLIDO1, 
                                                            EMP_APELLIDO2, EMP_DIRECCION, EMP_TELEFONO) VALUES
                                                        (@EMP_NOMBRE, @EMP_APELLIDO1, @EMP_APELLIDO2, @EMP_DIRECCION, @EMP_TELEFONO )", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@EMP_NOMBRE", empleado.EMP_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@EMP_APELLIDO1", empleado.EMP_APELLIDO1);
                sqlCommand.Parameters.AddWithValue("@EMP_APELLIDO2", empleado.EMP_APELLIDO2);
                sqlCommand.Parameters.AddWithValue("@EMP_DIRECCION", empleado.EMP_DIRECCION);
                sqlCommand.Parameters.AddWithValue("@EMP_TELEFONO", empleado.EMP_TELEFONO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();
            }


            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Put(Empleado empleado)
        {
            if (empleado == null)
                return BadRequest();

            if (ActualizarEmpleado(empleado))
                return Ok(empleado);
            else
                return InternalServerError();

        }

        private bool ActualizarEmpleado(Empleado empleado)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                  SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" UPDATE EMPLEADO SET
                                                            EMP_NOMBRE = @EMP_NOMBRE,
                                                            EMP_APELLIDO1 = @EMP_APELLIDO1,
                                                            EMP_APELLIDO2 = @EMP_APELLIDO2,
                                                            EMP_DIRECCION = @EMP_DIRECCION,
                                                            EMP_TELEFONO = @EMP_TELEFONO
                                                          WHERE EMP_CODIGO = @EMP_CODIGO ", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@EMP_CODIGO", empleado.EMP_CODIGO);
                sqlCommand.Parameters.AddWithValue("@EMP_NOMBRE", empleado.EMP_NOMBRE);
                sqlCommand.Parameters.AddWithValue("@EMP_APELLIDO1", empleado.EMP_APELLIDO1);
                sqlCommand.Parameters.AddWithValue("@EMP_APELLIDO2", empleado.EMP_APELLIDO2);
                sqlCommand.Parameters.AddWithValue("@EMP_DIRECCION", empleado.EMP_DIRECCION);
                sqlCommand.Parameters.AddWithValue("@EMP_TELEFONO", empleado.EMP_TELEFONO);

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

            if (EliminarEmpleado(id))
                return Ok(id);
            else
                return InternalServerError();
        }

        private bool EliminarEmpleado(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@" DELETE EMPLEADO
                                                          WHERE EMP_CODIGO = @EMP_CODIGO ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@EMP_CODIGO", id);
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
