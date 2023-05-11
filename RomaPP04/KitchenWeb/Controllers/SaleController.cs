using System.Data;
using KitchenWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace KitchenWeb.Controllers;

public class SaleController : Controller
{
    private readonly KitchenDbContext _context;
    private readonly string connectionString;

    public SaleController(KitchenDbContext context)
    {
        _context = context;
        connectionString = @"Data Source=HOME-PC;Initial Catalog=FinishKitchen;Integrated Security=True;TrustServerCertificate=True;MultiSubnetFailover=True";
    }
    // GET
    [HttpGet("/ReadSaleList")]
    public async Task<IActionResult> ReadSaleList()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sqlExpression = "ReadSale";
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
    public async Task<IActionResult> CreateProduction(int product, int count, DateTime date, int empl)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("CreateSale", connection))
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
    private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
        SqlDataReader reader)
    {
        var result = new Dictionary<string, object>();
        foreach (var col in cols)
            result.Add(col, reader[col]);
        return result;
    }
}