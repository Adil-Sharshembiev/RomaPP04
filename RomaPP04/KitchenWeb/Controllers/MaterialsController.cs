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
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace KitchenWeb.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly KitchenDbContext _context;
        private readonly string connectionString;

        public MaterialsController(KitchenDbContext context)
        {
            _context = context;
            connectionString = @"Data Source=DESKTOP-3A56OLQ;Initial Catalog=AccountingWeb;Integrated Security=True;TrustServerCertificate=True;MultiSubnetFailover=True";
        }
        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
                                                SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }
        // GET: Materials
        [HttpGet ("/GetMaterialsList")]
        public async Task<IActionResult> GetMaterialsList()
        {
            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
                connection.Open();
                string sqlExpression = "ReadMaterials";
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

        [HttpGet ("/GetUnitList")]
        public async Task<IActionResult> GetUnitList()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) 
                {
                    connection.Open();
                    string sqlExpression = "Unit";
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
        
        
        [HttpPost("/GetMaterialById")]
        public async Task<IActionResult> GetMaterialById([FromBody] int id)
        {
            Stream buffer = new MemoryStream();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlExpression = "ReadMaterialsById";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = id
                };
                command.Parameters.Add(idParam);

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
        [HttpPost("/CreateMaterials")]
        public async Task<IActionResult> CreateMaterials([FromForm] string name, int unit, double count, double price)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression = "CreateMaterials";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter nameParam = new SqlParameter
                    {
                        ParameterName = "@name",
                        Value = name
                    };
                    command.Parameters.Add(nameParam);
                    SqlParameter unitParam = new SqlParameter
                    {
                        ParameterName = "@unit",
                        Value = unit
                    };
                    command.Parameters.Add(unitParam);
                    SqlParameter countParam = new SqlParameter
                    {
                        ParameterName = "@count",
                        Value = count
                    };
                    command.Parameters.Add(countParam);
                    SqlParameter priceParam = new SqlParameter
                    {
                        ParameterName = "@price",
                        Value = price
                    };
                    command.Parameters.Add(priceParam);
                    var reader = command.ExecuteScalar();
                    return new JsonResult(new
                    {
                        status = 1,
                        message = "Успешное добавление"
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
        [HttpPost("/DeleteMaterialById")]
        public async Task<IActionResult> DeleteMaterialById([FromForm] int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression = "DelMaterials";
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
        [HttpPost("/UpdateMaterials")]
        public async Task<IActionResult> UpdateMaterials([FromForm] int id,string name, int unit, double count, double price)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression = "UpdateMaterials";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter idParam = new SqlParameter
                    {
                        ParameterName = "@id",
                        Value = id
                    };
                    command.Parameters.Add(idParam);
                    SqlParameter nameParam = new SqlParameter
                    {
                        ParameterName = "@name",
                        Value = name
                    };
                    command.Parameters.Add(nameParam);
                    SqlParameter unitParam = new SqlParameter
                    {
                        ParameterName = "@unit",
                        Value = unit
                    };
                    command.Parameters.Add(unitParam);
                    SqlParameter countParam = new SqlParameter
                    {
                        ParameterName = "@count",
                        Value = count
                    };
                    command.Parameters.Add(countParam);
                    SqlParameter priceParam = new SqlParameter
                    {
                        ParameterName = "@price",
                        Value = price
                    };
                    command.Parameters.Add(priceParam);
                    var reader = command.ExecuteScalar();
                    return new JsonResult(new
                    {
                        status = 1,
                        message = "Успешное изменение"
                    });
                }
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
        // GET: Materials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .Include(m => m.UnitOfMeasurementNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // GET: Materials/Create
        public IActionResult Create()
        {
            ViewData["UnitOfMeasurement"] = new SelectList(_context.UnitsOfMeasurements, "Id", "Id");
            return View();
        }

        // POST: Materials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UnitOfMeasurement,Count,Price,Cost")] Material material)
        {
            if (ModelState.IsValid)
            {
                _context.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UnitOfMeasurement"] = new SelectList(_context.UnitsOfMeasurements, "Id", "Id", material.UnitOfMeasurement);
            return View(material);
        }

        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            ViewData["UnitOfMeasurement"] = new SelectList(_context.UnitsOfMeasurements, "Id", "Id", material.UnitOfMeasurement);
            return View(material);
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UnitOfMeasurement,Count,Price,Cost")] Material material)
        {
            if (id != material.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UnitOfMeasurement"] = new SelectList(_context.UnitsOfMeasurements, "Id", "Id", material.UnitOfMeasurement);
            return View(material);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .Include(m => m.UnitOfMeasurementNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Materials == null)
            {
                return Problem("Entity set 'KitchenDbContext.Materials'  is null.");
            }
            var material = await _context.Materials.FindAsync(id);
            if (material != null)
            {
                _context.Materials.Remove(material);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialExists(int id)
        {
          return (_context.Materials?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
