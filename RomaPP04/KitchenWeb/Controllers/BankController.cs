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
        [HttpPost("/CreateCredit")]
        public async Task<IActionResult> CreateCredit([FromForm] string summa, string year, double percent, double fine, DateTime date, String description)
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
                    SqlParameter descriptionParam = new SqlParameter
                    {
                        ParameterName = "@description",
                        Value = description
                    };
                    command.Parameters.Add(descriptionParam);

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

        [HttpPost("/GetCreditPay")]
        public async Task<IActionResult> GetCreditPay([FromForm] int credit_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression = "ReadPayByCredit_Id";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter creditIdParam = new SqlParameter
                    {
                        ParameterName = "@credit_id",
                        Value = credit_id
                    };
                    command.Parameters.Add(creditIdParam);

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


        [HttpPost("/PayCredit")]
        public async Task<IActionResult> PaySalary([FromForm] DateTime date, int credit_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("CreatePay", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "CreatePay";
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@credit_id", credit_id);
                        command.Parameters.Add("@rez", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@total", SqlDbType.Float).Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();
                        int rezValue = (int)command.Parameters["@rez"].Value;
                        double totalValue = (double)command.Parameters["@total"].Value;
                        if (rezValue == 1)
                        {
                            return new JsonResult(new
                            {
                                status = 1,
                                message = "Выплачено: "+totalValue
                            });
                        }
                        else
                        {
                            return new JsonResult(new
                            {
                                status = 0,
                                message = "Недостаточное денег в бюджете! Необходимо "+ totalValue,
                            });
                        }

                    }
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
