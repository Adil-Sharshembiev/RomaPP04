using KitchenWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace KitchenWeb.Controllers;

public class ProductsController : Controller
{
    private readonly KitchenDbContext _context;
    private readonly string connectionString;

    public ProductsController(KitchenDbContext context)
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
    [HttpGet ("/GetProductsList")]
    public async Task<IActionResult> GetMaterialsList()
    {
        using (SqlConnection connection = new SqlConnection(connectionString)) 
        {
            connection.Open();
            string sqlExpression = "ReadProducts";
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
    
    
}