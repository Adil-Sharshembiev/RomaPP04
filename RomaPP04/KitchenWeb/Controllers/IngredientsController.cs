using KitchenWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace KitchenWeb.Controllers;

public class IngredientsController : Controller
{
    private readonly KitchenDbContext _context;
    private readonly string connectionString;

    public IngredientsController(KitchenDbContext context)
    {
        _context = context;
        connectionString = @"Data Source=HOME-PC;Initial Catalog=FinishKitchen;Integrated Security=True;TrustServerCertificate=True;MultiSubnetFailover=True";
    }
    private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
        SqlDataReader reader)
    {
        var result = new Dictionary<string, object>();
        foreach (var col in cols)
            result.Add(col, reader[col]);
        return result;
    }
    [HttpPost("/GetMatIngredients")]
    public async Task<IActionResult> GetMatIngr(int product)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlExpression = "ReadIngredients";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@product",
                    Value = product
                };
                command.Parameters.Add(idParam);

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
    
    
    [HttpPost("/CreateIngredients")]
    public async Task<IActionResult> CreateIngredients( int product, int material, double count)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlExpression = "CreateIngredients";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter productParam = new SqlParameter
                {
                    ParameterName = "@product",
                    Value = product
                };
                command.Parameters.Add(productParam);
                SqlParameter materialParam = new SqlParameter
                {
                    ParameterName = "@material",
                    Value = material
                };
                command.Parameters.Add(materialParam);
                SqlParameter countParam = new SqlParameter
                {
                    ParameterName = "@count",
                    Value = count
                };
                command.Parameters.Add(countParam);

                var reader = command.ExecuteReader();
                return new JsonResult(new
                {
                    status = 1,
                    message = "Успешное добавление",
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
    [HttpPost("/UpdateIngredients")]
    public async Task<IActionResult> UpdateIngredients( int id, int product, int material, double count)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlExpression = "UpdateIngredients";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = id
                };
                command.Parameters.Add(idParam);
                SqlParameter productParam = new SqlParameter
                {
                    ParameterName = "@product",
                    Value = product
                };
                command.Parameters.Add(productParam);
                SqlParameter materialParam = new SqlParameter
                {
                    ParameterName = "@material",
                    Value = material
                };
                command.Parameters.Add(materialParam);
                SqlParameter countParam = new SqlParameter
                {
                    ParameterName = "@count",
                    Value = count
                };
                command.Parameters.Add(countParam);

                var reader = command.ExecuteReader();
                return new JsonResult(new
                {
                    status = 1,
                    message = "Успешное обновление",
                });
            }
        }
        catch (Exception e)
        {
            return new JsonResult(new
            {
                status = 2,
                message = "Не удалось обновить"
            });
        }
    }
    [HttpPost("/DeleteIngredients")]
    public async Task<IActionResult> DeleteIngredients( int id)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlExpression = "DelIngredients";
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
    
}