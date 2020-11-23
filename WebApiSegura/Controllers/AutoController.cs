using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/auto")]
    public class AutoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            Auto auto = new Auto();
            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(
                        @"SELECT AUTO_CODIGO
                        , MAR_CODIGO
                        , MOD_CODIGO
                        , COMB_CODIGO
                        , SUC_CODIGO
                        , AUTO_CANTIDAD
                        , AUTO_PRECIO
                        FROM AUTOS
                        WHERE AUTO_CODIGO = @AUTO_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@AUTO_CODIGO", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        auto.AUTO_CODIGO = sqlDataReader.GetInt32(0);
                        auto.MAR_CODIGO = sqlDataReader.GetInt32(1);
                        auto.MOD_CODIGO = sqlDataReader.GetInt32(2);
                        auto.COMB_CODIGO = sqlDataReader.GetInt32(3);
                        auto.SUC_CODIGO = sqlDataReader.GetInt32(4);
                        auto.AUTO_CANTIDAD = sqlDataReader.GetString(5);
                        auto.AUTO_PRECIO = (decimal)sqlDataReader.GetSqlDecimal(6);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(auto);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Auto> autos = new List<Auto>();

            try
            {
                using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {

                    SqlCommand sqlCommand = new SqlCommand(
                        @"SELECT AUTO_CODIGO
                        , MAR_CODIGO
                        , MOD_CODIGO
                        , COMB_CODIGO
                        , SUC_CODIGO
                        , AUTO_CANTIDAD
                        , AUTO_PRECIO
                        FROM AUTOS", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Auto auto = new Auto()
                        {
                            AUTO_CODIGO = sqlDataReader.GetInt32(0),
                            MAR_CODIGO = sqlDataReader.GetInt32(1),
                            MOD_CODIGO = sqlDataReader.GetInt32(2),
                            COMB_CODIGO = sqlDataReader.GetInt32(3),
                            SUC_CODIGO = sqlDataReader.GetInt32(4),
                            AUTO_CANTIDAD = sqlDataReader.GetString(5),
                            AUTO_PRECIO = (decimal)sqlDataReader.GetSqlDecimal(6)
                        };
                        autos.Add(auto);
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(autos);
        }

        [HttpPost]
        [Route("ingresar")]
        public IHttpActionResult Ingresar(Auto auto)
        {
            if (auto == null)
                return BadRequest();

            if (RegistrarAuto(auto))
                return Ok(auto);
            else
                return InternalServerError();
        }

        private bool RegistrarAuto(Auto auto)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                   SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(
                    @"INSERT INTO AUTOS
                    (MAR_CODIGO
                    , MOD_CODIGO
                    , COMB_CODIGO
                    , SUC_CODIGO
                    , AUTO_CANTIDAD
                    , AUTO_PRECIO) 
                    VALUES(
                    @MAR_CODIGO
                    , @MOD_CODIGO
                    , @COMB_CODIGO
                    , @SUC_CODIGO
                    , @AUTO_CANTIDAD
                    , @AUTO_PRECIO)", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@MAR_CODIGO", auto.MAR_CODIGO);
                sqlCommand.Parameters.AddWithValue("@MOD_CODIGO", auto.MOD_CODIGO);
                sqlCommand.Parameters.AddWithValue("@COMB_CODIGO", auto.COMB_CODIGO);
                sqlCommand.Parameters.AddWithValue("@SUC_CODIGO", auto.SUC_CODIGO);
                sqlCommand.Parameters.AddWithValue("@AUTO_CANTIDAD", auto.AUTO_CANTIDAD);
                sqlCommand.Parameters.AddWithValue("@AUTO_PRECIO", auto.AUTO_PRECIO);

                sqlConnection.Open();

                int filasAfectadas = sqlCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                    resultado = true;

                sqlConnection.Close();
            }
            return resultado;
        }

        [HttpPut]
        public IHttpActionResult Put(Auto auto)
        {
            if (auto == null)
                return BadRequest();

            if (ActualizarAuto(auto))
                return Ok(auto);
            else
                return InternalServerError();
        }

        private bool ActualizarAuto(Auto auto)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                  SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {

                SqlCommand sqlCommand = new SqlCommand(
                    @"UPDATE AUTOS SET
                    MAR_CODIGO = @MAR_CODIGO ,
                    MOD_CODIGO = @MOD_CODIGO ,
                    COMB_CODIGO = @COMB_CODIGO ,
                    SUC_CODIGO = @SUC_CODIGO ,
                    AUTO_CANTIDAD = @AUTO_CANTIDAD ,
                    AUTO_PRECIO = @AUTO_PRECIO
                    WHERE AUTO_CODIGO = @AUTO_CODIGO
                    ", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@AUTO_CODIGO", auto.AUTO_CODIGO);
                sqlCommand.Parameters.AddWithValue("@MAR_CODIGO", auto.MAR_CODIGO);
                sqlCommand.Parameters.AddWithValue("@MOD_CODIGO", auto.MOD_CODIGO);
                sqlCommand.Parameters.AddWithValue("@COMB_CODIGO", auto.COMB_CODIGO);
                sqlCommand.Parameters.AddWithValue("@SUC_CODIGO", auto.SUC_CODIGO);
                sqlCommand.Parameters.AddWithValue("@AUTO_CANTIDAD", auto.AUTO_CANTIDAD);
                sqlCommand.Parameters.AddWithValue("@AUTO_PRECIO", auto.AUTO_PRECIO);

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

            if (EliminarAuto(id))
                return Ok(id);
            else
                return InternalServerError();
        }

        private bool EliminarAuto(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new
                SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(@"
                    DELETE AUTOS
                    WHERE AUTO_CODIGO = @AUTO_CODIGO", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@AUTO_CODIGO", id);

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
