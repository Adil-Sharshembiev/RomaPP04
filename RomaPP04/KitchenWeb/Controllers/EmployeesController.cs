using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KitchenWeb.Repository;
using KitchenWeb.Repository.Models;
using Microsoft.Data.SqlClient;

namespace KitchenWeb.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly KitchenDbContext _context;
        private readonly string connectionString;
        private readonly IConfiguration _configuration;

        public EmployeesController(IConfiguration configuration)
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
        [HttpGet ("/GetEmployeessList")]
        public async Task<IActionResult> GetEmployeessList()
        {
            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
                connection.Open();
                string sqlExpression = "SP_ReadEmployees";
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
}
