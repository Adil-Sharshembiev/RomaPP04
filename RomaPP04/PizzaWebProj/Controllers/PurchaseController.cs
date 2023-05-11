using System.Data;
using KitchenWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace KitchenWeb.Controllers;

public class PurchaseController : Controller
{
    private readonly string connectionString;
    private readonly IConfiguration _configuration;

    public PurchaseController(IConfiguration configuration)
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

    [HttpGet("/GetPurchaseList")]
    public async Task<IActionResult> GetPurchaseList()
    {
        Stream buffer = new MemoryStream();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sqlExpression = "ReadPurchase";
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

    [HttpPost("/CreatePurchase")]
    public async Task<IActionResult> CreatePurchase([FromForm] int material, string count, string price, DateTime date,
        int employee)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            count = count.Replace(".", ",");
            double c = Convert.ToDouble(count);
            price = price.Replace(".", ",");
            double p = Convert.ToDouble(price);
            connection.Open();
            using (SqlCommand command = new SqlCommand("CreatePurchase", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "CreatePurchase";
                command.Parameters.AddWithValue("@material", material);
                command.Parameters.AddWithValue("@count", c);
                command.Parameters.AddWithValue("@price", p);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@employee", employee);
                command.Parameters.Add("@rez", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                int rezValue = (int)command.Parameters["@rez"].Value;
                if (rezValue == 0)
                {
                    return new JsonResult(new
                    {
                        status = 2,
                        message = "Не удалось добавить"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        status = 1,
                        message = "Успешное добавление"
                    });
                }
            }
        }
    }

    [HttpPost("/UpdatePurchase")]
    public async Task<IActionResult> UpdatePurchase([FromForm] int id, int material, string count, double price,
        DateTime date,
        int employee)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            count = count.Replace(".", ",");
            double c = Convert.ToDouble(count);
            connection.Open();
            using (SqlCommand command = new SqlCommand("UpdatePurchase", connection))
            {
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "UpdatePurchase";
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@matId", material);
                    command.Parameters.AddWithValue("@count", c);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@employeeId", employee);
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
}