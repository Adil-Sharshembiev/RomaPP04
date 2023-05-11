using System.Data;
using KitchenWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace KitchenWeb.Controllers;

public class BudgetController : Controller
{
    private readonly string connectionString;
    private readonly IConfiguration _configuration;

    public BudgetController(IConfiguration configuration)
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

    [HttpGet("/GetBudgetList")]
    public async Task<IActionResult> GetEmployeessList()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try
            {
                string sqlExpression = "SP_GetBudget";
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

    [HttpPost("/UpdateBudget")]
    public async Task<IActionResult> UpdateBudget([FromForm] string budget, int bonus)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            budget = budget.Replace(".", ",");
            double b = Convert.ToDouble(budget);
            connection.Open();
            using (SqlCommand command = new SqlCommand("SP_EditPurchase", connection))
            {
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "UpdateBudget";
                    command.Parameters.AddWithValue("@budget", b);
                    command.Parameters.AddWithValue("@bonus", bonus);
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