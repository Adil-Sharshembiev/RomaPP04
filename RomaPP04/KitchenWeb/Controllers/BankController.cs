using KitchenWeb.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KitchenWeb.Controllers
{
    public class BankController : Controller
    {
        private readonly KitchenDbContext _context;
        private readonly string connectionString;
        private readonly IConfiguration _configuration;

        public BankController(IConfiguration configuration)
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


        [HttpGet("/GetCredit")]
        public async Task<IActionResult> GetSalary()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression = "ReadCredit";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
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
        [HttpPost("/CreateCredit")]
        public async Task<IActionResult> CreateCredit([FromForm] string summa, string year, double percent, double fine, DateTime date)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    summa = summa.Replace(".", ",");
                    year = year.Replace(".", ",");

                    double s = Convert.ToDouble(summa);
                    double y = Convert.ToDouble(year);
                    connection.Open();
                    string sqlExpression = "CreateCredit";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter sumParam = new SqlParameter
                    {
                        ParameterName = "@sum",
                        Value = s
                    };
                    command.Parameters.Add(sumParam);
                    SqlParameter yearParam = new SqlParameter
                    {
                        ParameterName = "@year",
                        Value = y
                    };
                    command.Parameters.Add(yearParam);
                    SqlParameter percentParam = new SqlParameter
                    {
                        ParameterName = "@percent",
                        Value = percent
                    };
                    command.Parameters.Add(percentParam);
                    SqlParameter fineParam = new SqlParameter
                    {
                        ParameterName = "@fine",
                        Value = fine
                    };
                    command.Parameters.Add(fineParam);
                    SqlParameter dateParam = new SqlParameter
                    {
                        ParameterName = "@date",
                        Value = date
                    };
                    command.Parameters.Add(dateParam);

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
}
