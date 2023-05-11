using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KitchenWeb.Repository;
using KitchenWeb.Repository.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace KitchenWeb.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;

        public MaterialsController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
            SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }

        // GET: Materials
        [HttpGet("/GetMaterialsList")]
        public async Task<IActionResult> GetMaterialsList()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlExpression = "ReadMaterials";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var reader = command.ExecuteReader();


                var results = new List<Dictionary<string, object>>();
                var cols = new List<string>();
                for (var i = 0; i < reader.FieldCount; i++)
                    cols.Add(reader.GetName(i));
                while (reader.Read())
                    results.Add(SerializeRow(cols, reader));
                return Ok(results);
            }
        }

        [HttpGet("/GetUnitList")]
        public async Task<IActionResult> GetUnitList()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression = "Unit";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var reader = command.ExecuteReader();
                    var results = new List<Dictionary<string, object>>();
                    var cols = new List<string>();
                    for (var i = 0; i < reader.FieldCount; i++)
                        cols.Add(reader.GetName(i));
                    while (reader.Read())
                        results.Add(SerializeRow(cols, reader));
                    return new JsonResult(new
                    {
                        status = 1,
                        message = "Успех",
                        result = results
                    });
                }
            }
            catch (Exception e)
            {
                return new JsonResult(new
                {
                    status = 2,
                    message = "Ошибка"
                });
            }
        }


        [HttpPost("/GetMaterialById")]
        public async Task<IActionResult> GetMaterialById([FromBody] int id)
        {
            Stream buffer = new MemoryStream();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlExpression = "ReadMaterialsById";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = id
                };
                command.Parameters.Add(idParam);

                var reader = command.ExecuteReader();


                var results = new List<Dictionary<string, object>>();
                var cols = new List<string>();
                for (var i = 0; i < reader.FieldCount; i++)
                    cols.Add(reader.GetName(i));
                while (reader.Read())
                    results.Add(SerializeRow(cols, reader));
                return Ok(results);
            }
        }

        [HttpPost("/CreateMaterials")]
        public async Task<IActionResult> CreateMaterials([FromForm] string name, int unit, string count, string price)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    count = count.Replace(".", ",");
                    price = count.Replace(".", ",");

                    double c = Convert.ToDouble(count);
                    double p = Convert.ToDouble(price);

                    connection.Open();
                    string sqlExpression = "CreateMaterials";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter nameParam = new SqlParameter
                    {
                        ParameterName = "@name",
                        Value = name
                    };
                    command.Parameters.Add(nameParam);
                    SqlParameter unitParam = new SqlParameter
                    {
                        ParameterName = "@unit",
                        Value = unit
                    };
                    command.Parameters.Add(unitParam);
                    SqlParameter countParam = new SqlParameter
                    {
                        ParameterName = "@count",
                        Value = c
                    };
                    command.Parameters.Add(countParam);
                    SqlParameter priceParam = new SqlParameter
                    {
                        ParameterName = "@price",
                        Value = p
                    };
                    command.Parameters.Add(priceParam);
                    var reader = command.ExecuteScalar();
                    return new JsonResult(new
                    {
                        status = 1,
                        message = "Успешное добавление"
                    });
                }
            }
            catch (Exception e)
            {
                return new JsonResult(new
                {
                    status = 2,
                    message = "Не удалось добавить"
                });
            }
        }

        [HttpPost("/DeleteMaterialById")]
        public async Task<IActionResult> DeleteMaterialById([FromForm] int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression = "DelMaterials";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter idParam = new SqlParameter
                    {
                        ParameterName = "@id",
                        Value = id
                    };
                    command.Parameters.Add(idParam);

                    var reader = command.ExecuteReader();
                    return new JsonResult(new
                    {
                        status = 1,
                        message = "Успешное удаление"
                    });
                }
            }
            catch (Exception e)
            {
                return new JsonResult(new
                {
                    status = 2,
                    message = "Не удалось удалить"
                });
            }
        }

        [HttpPost("/UpdateMaterials")]
        public async Task<IActionResult> UpdateMaterials([FromForm] int id, string name, int unit, string count,
            string price)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    count = count.Replace(".", ",");
                    price = count.Replace(".", ",");

                    double c = Convert.ToDouble(count);
                    double p = Convert.ToDouble(price);

                    connection.Open();
                    string sqlExpression = "UpdateMaterials";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter idParam = new SqlParameter
                    {
                        ParameterName = "@id",
                        Value = id
                    };
                    command.Parameters.Add(idParam);
                    SqlParameter nameParam = new SqlParameter
                    {
                        ParameterName = "@name",
                        Value = name
                    };
                    command.Parameters.Add(nameParam);
                    SqlParameter unitParam = new SqlParameter
                    {
                        ParameterName = "@unit",
                        Value = unit
                    };
                    command.Parameters.Add(unitParam);
                    SqlParameter countParam = new SqlParameter
                    {
                        ParameterName = "@count",
                        Value = c
                    };
                    command.Parameters.Add(countParam);
                    SqlParameter priceParam = new SqlParameter
                    {
                        ParameterName = "@price",
                        Value = p
                    };
                    command.Parameters.Add(priceParam);
                    var reader = command.ExecuteScalar();
                    return new JsonResult(new
                    {
                        status = 1,
                        message = "Успешное изменение"
                    });
                }
            }
            catch (Exception e)
            {
                return new JsonResult(new
                {
                    status = 2,
                    message = "Не удалось изменить"
                });
            }
        }
    }
}