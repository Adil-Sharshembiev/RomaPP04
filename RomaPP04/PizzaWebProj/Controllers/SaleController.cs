using System.Data;
using KitchenWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace KitchenWeb.Controllers;

public class SaleController : Controller
{
    private readonly string connectionString;
    private readonly IConfiguration _configuration;

    public SaleController(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    // GET
    [HttpGet("/ReadSaleList")]
    public async Task<IActionResult> ReadSaleList()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sqlExpression = "SP_GetSale";
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

    [HttpPost("/CreateSale")]
    public async Task<IActionResult> CreateProduction([FromForm] int product, int count, DateTime date, int empl)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("SP_InsertSale", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "CreateSale";
                command.Parameters.AddWithValue("@product", product);
                command.Parameters.AddWithValue("@count", count);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@empl", empl);
                command.Parameters.Add("@rez", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@countProd", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                int rezValue = (int)command.Parameters["@rez"].Value;
                int rezCount = (int)command.Parameters["@countProd"].Value;
                if (rezValue == 1)
                {
                    return new JsonResult(new
                    {
                        status = 1,
                        message = "Добававлено"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        status = 2,
                        message = "недостаточное количество материала",
                        count = rezCount
                    });
                }
            }
        }
    }

    [HttpPost("/UpdateSale")]
    public async Task<IActionResult> UpdateSale([FromForm] int id, int product, string count, int price, int sum,
        DateTime date,
        int emploee)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            count = count.Replace(".", ",");
            double c = Convert.ToDouble(count);
            connection.Open();
            using (SqlCommand command = new SqlCommand("SP_EditSale", connection))
            {
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "UpdateSale";
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@product", product);
                    command.Parameters.AddWithValue("@count", c);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@sum", sum);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@emploee", emploee);
                    command.ExecuteNonQuery();
                    return new JsonResult(new
                    {
                        status = 1,
                        message = "Изменено"
                    });
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

    private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
        SqlDataReader reader)
    {
        var result = new Dictionary<string, object>();
        foreach (var col in cols)
            result.Add(col, reader[col]);
        return result;
    }
}