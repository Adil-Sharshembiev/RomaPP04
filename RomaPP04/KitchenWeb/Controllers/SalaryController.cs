using System.Data;
using KitchenWeb.Repository;
using KitchenWeb.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace KitchenWeb.Controllers;

public class SalaryController : Controller
{
    private readonly KitchenDbContext _context;
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
                    string sqlExpression = "ReadSalary";
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

    [HttpPost("/PaySalary")]
    public async Task<IActionResult> PaySalary([FromForm] int year, int month)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("paySalary", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "paySalary";
                    command.Parameters.AddWithValue("@year", year);
                    command.Parameters.AddWithValue("@month", month);
                    command.Parameters.Add("@rez", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.ExecuteNonQuery();
                    int rezValue = (int)command.Parameters["@rez"].Value;
                    if (rezValue == 1)
                    {
                        return new JsonResult(new
                        {
                            status = 1,
                            message = "Выплачено"
                        });
                    }
                    else if (rezValue == 0) 
                    {
                        return new JsonResult(new
                        {
                            status = 0,
                            message = "Недостаточное денег в бюджете",
                        });
                    }
                    else
                    {
                        return new JsonResult(new
                        {
                            status = 2,
                            message = "Ранее уже выплачено",
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

    [HttpPost("/UpdateSalary")]
    public async Task<IActionResult> UpdateSalary([FromForm] int id, string total)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UpdateSalary", connection))
                {
                    total = total.Replace(".", ",");
                    double c = Convert.ToDouble(total);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "UpdateSalary";
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@salary", c);
                    command.Parameters.Add("@rez", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.ExecuteNonQuery();
                    int rezValue = (int)command.Parameters["@rez"].Value;
                    if (rezValue == 1)
                    {
                        return new JsonResult(new
                        {
                            status = 1,
                            message = "Успешное изменение"
                        });
                    }
                    else
                    {
                        return new JsonResult(new
                        {
                            status = 0,
                            message = "Уже выплачено! Изменить невозможно!",
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