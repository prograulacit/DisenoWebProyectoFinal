using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Configuration;
using System.Data.SqlClient;
using WebApiSegura.Models;


namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/renta")]
    public class RentaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Renta renta = new Renta();

            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager
                   .ConnectionStrings["Reservas"].ConnectionString))
                {
                    SqlCommand sqlCommand =
                        new SqlCommand(
                            @"SELECT 
                            REN_CODIGO
                            , USU_IDENTIFICACION
                            , AUTO_CODIGO
                            , EMP_CODIGO
                            , REN_DESCRIPCION
                            , REN_CANTIDAD
                            , REN_PRECIO
                            , REN_FEC_RETIRO
                            , REN_FEC_RETORNO
                            FROM RENTA
                            WHERE REN_CODIGO = @REN_CODIGO;", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@REN_CODIGO", id);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        renta.REN_CODIGO = sqlDataReader.GetInt32(0);
                        renta.USU_IDENTIFICACION = sqlDataReader.GetString(1);
                        renta.AUTO_CODIGO = sqlDataReader.GetInt32(2);
                        renta.EMP_CODIGO = sqlDataReader.GetInt32(3);
                        renta.REN_DESCRIPCION = sqlDataReader.GetString(4);
                        renta.REN_CANTIDAD = sqlDataReader.GetInt32(5);
                        renta.REN_PRECIO = sqlDataReader.GetDecimal(6);
                        renta.REN_FEC_RETIRO = sqlDataReader.GetDateTime(7);
                        renta.REN_FEC_RETORNO = sqlDataReader.GetDateTime(8);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(renta);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Renta> reservas = new List<Renta>();
            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager
                   .ConnectionStrings["Reservas"].ConnectionString))
                {
                    SqlCommand sqlCommand =
                        new SqlCommand(
                            @"SELECT 
                            REN_CODIGO
                            , USU_IDENTIFICACION
                            , AUTO_CODIGO
                            , EMP_CODIGO
                            , REN_DESCRIPCION
                            , REN_CANTIDAD
                            , REN_PRECIO
                            , REN_FEC_RETIRO
                            , REN_FEC_RETORNO
                            FROM RENTA;", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Renta renta = new Renta()
                        {
                            REN_CODIGO = sqlDataReader.GetInt32(0),
                            USU_IDENTIFICACION = sqlDataReader.GetString(1),
                            AUTO_CODIGO = sqlDataReader.GetInt32(2),
                            EMP_CODIGO = sqlDataReader.GetInt32(3),
                            REN_DESCRIPCION = sqlDataReader.GetString(4),
                            REN_CANTIDAD = sqlDataReader.GetInt32(5),
                            REN_PRECIO = sqlDataReader.GetDecimal(6),
                            REN_FEC_RETIRO = sqlDataReader.GetDateTime(7),
                            REN_FEC_RETORNO = sqlDataReader.GetDateTime(8),
                        };
                        reservas.Add(renta);
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
        public IHttpActionResult Ingresar(Renta renta)
        {
            if (renta == null)
                return BadRequest();

            if (registrarReserva(renta))
                return Ok(renta);
            else
                return InternalServerError();

        }

        private bool registrarReserva(Renta renta)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager
                   .ConnectionStrings["Reservas"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(
                    @"INSERT INTO RENTA (
                            USU_IDENTIFICACION
                            , AUTO_CODIGO
                            , EMP_CODIGO
                            , REN_DESCRIPCION
                            , REN_CANTIDAD
                            , REN_PRECIO
                            , REN_FEC_RETIRO
                            , REN_FEC_RETORNO)
                    VALUES(
                            @USU_IDENTIFICACION
                            , @AUTO_CODIGO
                            , @EMP_CODIGO
                            , @REN_DESCRIPCION
                            , @REN_CANTIDAD
                            , @REN_PRECIO
                            , @REN_FEC_RETIRO
                            , @REN_FEC_RETORNO)", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@USU_IDENTIFICACION", renta.USU_IDENTIFICACION);
                sqlCommand.Parameters.AddWithValue("@AUTO_CODIGO", renta.AUTO_CODIGO);
                sqlCommand.Parameters.AddWithValue("@EMP_CODIGO", renta.EMP_CODIGO);
                sqlCommand.Parameters.AddWithValue("@REN_DESCRIPCION", renta.REN_DESCRIPCION);
                sqlCommand.Parameters.AddWithValue("@REN_CANTIDAD", renta.REN_CANTIDAD);
                sqlCommand.Parameters.AddWithValue("@REN_PRECIO", renta.REN_PRECIO);
                sqlCommand.Parameters.AddWithValue("@REN_FEC_RETIRO", renta.REN_FEC_RETIRO);
                sqlCommand.Parameters.AddWithValue("@REN_FEC_RETORNO", renta.REN_FEC_RETORNO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();
            }
            return resultado;
        }

        public IHttpActionResult Put(Renta renta)
        {
            if (renta == null)
                return BadRequest();

            if (actualizarReserva(renta))
                return Ok(renta);
            else
                return InternalServerError();

        }

        private bool actualizarReserva(Renta renta)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                  SqlConnection(ConfigurationManager
                  .ConnectionStrings["Reservas"].ConnectionString))
            {
                SqlCommand sqlCommand =
                    new SqlCommand(
                        @"UPDATE RENTA SET
                            USU_IDENTIFICACION = @USU_IDENTIFICACION,
                            AUTO_CODIGO = @AUTO_CODIGO,
                            EMP_CODIGO = @EMP_CODIGO,
                            REN_DESCRIPCION = @REN_DESCRIPCION,
                            REN_CANTIDAD = @REN_CANTIDAD,
                            REN_PRECIO = @REN_PRECIO,
                            REN_FEC_RETIRO = @REN_FEC_RETIRO,
                            REN_FEC_RETORNO = @REN_FEC_RETORNO
                        WHERE REN_CODIGO = @REN_CODIGO;", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@REN_CODIGO", renta.REN_CODIGO);
                sqlCommand.Parameters.AddWithValue("@USU_IDENTIFICACION", renta.USU_IDENTIFICACION);
                sqlCommand.Parameters.AddWithValue("@AUTO_CODIGO", renta.AUTO_CODIGO);
                sqlCommand.Parameters.AddWithValue("@EMP_CODIGO", renta.EMP_CODIGO);
                sqlCommand.Parameters.AddWithValue("@REN_DESCRIPCION", renta.REN_DESCRIPCION);
                sqlCommand.Parameters.AddWithValue("@REN_CANTIDAD", renta.REN_CANTIDAD);
                sqlCommand.Parameters.AddWithValue("@REN_PRECIO", renta.REN_PRECIO);
                sqlCommand.Parameters.AddWithValue("@REN_FEC_RETIRO", renta.REN_FEC_RETIRO);
                sqlCommand.Parameters.AddWithValue("@REN_FEC_RETORNO", renta.REN_FEC_RETORNO);

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

            if (eliminarReserva(id))
                return Ok(id);
            else
                return InternalServerError();
        }

        private bool eliminarReserva(int id)
        {
            try
            {
                bool resultado = false;

                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["Reservas"].ConnectionString))
                {
                    SqlCommand sqlCommand =
                        new SqlCommand(
                            @" DELETE RENTA
                        WHERE 
                        REN_CODIGO = @REN_CODIGO;", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@REN_CODIGO", id);
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
