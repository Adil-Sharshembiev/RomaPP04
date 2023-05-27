using System.Data;
using KitchenWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace KitchenWeb.Controllers;

public class ProductionController : Controller
{
    private readonly KitchenDbContext _context;
    private readonly string connectionString;
    private readonly IConfiguration _configuration;
    public ProductionController(IConfiguration configuration)
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
    [HttpGet("/ReadProductionList")]
    public async Task<IActionResult> ReadProductionList()
    {
        Stream buffer = new MemoryStream();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sqlExpression = "ReadProduction";
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
    [HttpPost("/CreateProduction")]
    public async Task<IActionResult> CreateProduction([FromForm] int product, string count, DateTime date, int employee)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("CreateProduction", connection))
            {
                count = count.Replace(".",",");
                double c = Convert.ToDouble(count);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "CreateProduction";
                command.Parameters.AddWithValue("@product", product);
                command.Parameters.AddWithValue("@count", c);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@employee", employee);
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
    [HttpPost("/UpdateProduction")]
    public async Task<IActionResult> UpdateProduction([FromForm] int id, int product, string count, DateTime date,
        int employee)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("UpdateProduction", connection))
            {
                count = count.Replace(".",",");
                double c = Convert.ToDouble(count);
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "UpdateProduction";
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@product", product);
                    command.Parameters.AddWithValue("@count", c);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@employee", employee);
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