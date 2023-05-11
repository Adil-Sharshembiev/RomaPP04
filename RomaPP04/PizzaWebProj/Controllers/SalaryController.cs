using System.Data;
using KitchenWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace KitchenWeb.Controllers;

public class SalaryController : Controller
{
    private readonly string connectionString;
    private readonly IConfiguration _configuration;

    public SalaryController(IConfiguration configuration)
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

    [HttpPost("/GetSalary")]
    public async Task<IActionResult> GetSalary([FromForm] int year, int month)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlExpression = "SP_GetSalary";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter yearParam = new SqlParameter
                {
                    ParameterName = "@year",
                    Value = year
                };
                command.Parameters.Add(yearParam);
                SqlParameter monthParam = new SqlParameter
                {
                    ParameterName = "@month",
                    Value = month
                };
                command.Parameters.Add(monthParam);

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
}