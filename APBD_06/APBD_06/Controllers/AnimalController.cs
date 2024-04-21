using APBD_06.Models;

namespace APBD_06.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;

[Route("api/animals")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly string _connectionString;

    public AnimalsController()
    {
        _connectionString = "Data Source = db-mssql;Initial Catalog=2019SBD;Integrated Security=True";
    }

    [HttpGet]
    public IActionResult GetAnimals([FromQuery] string orderBy = "name")
    {
        var allowedSortFields = new List<string> { "name", "description", "category", "area" };
        if (!allowedSortFields.Contains(orderBy.ToLower()))
        {
            return BadRequest($"Niepoprawna nazwa kolumny. Dozwolone nazwy: {string.Join(", ", allowedSortFields)}.");
        }

        var animals = new List<Animal>();
        var query = $"SELECT IdAnimal, Name, Description, Category, Area FROM Animal ORDER BY {orderBy}";

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    animals.Add(new Animal
                    {
                        IdAnimal = (int)reader["IdAnimal"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"] as string,
                        Category = reader["Category"].ToString(),
                        Area = reader["Area"].ToString()
                    });
                }
            }

            return Ok(animals);
        }
        catch (SqlException ex)
        {
            return StatusCode(500, "error bazy danych: " + ex.Message);
        }
    }

    [HttpPost]
    public IActionResult AddAnimal([FromBody] Animal animal)
    {
        var query = "INSERT INTO Animal (Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area)";
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", animal.Name);
                command.Parameters.AddWithValue("@Description", animal.Description);
                command.Parameters.AddWithValue("@Category", animal.Category);
                command.Parameters.AddWithValue("@Area", animal.Area);
                connection.Open();
                int result = command.ExecuteNonQuery();

                return result > 0 ? StatusCode(201) : BadRequest("no rows affected");
            }
        }
        catch (SqlException ex)
        {
            return StatusCode(500, "error bazy danych: " + ex.Message);
        }
    }


    [HttpPut("{idAnimal}")]
    public IActionResult UpdateAnimal(int idAnimal, [FromBody] Animal animal)
    {
        var query = $"UPDATE Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal";
    
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                command.Parameters.AddWithValue("@Name", animal.Name);
                command.Parameters.AddWithValue("@Description", animal.Description);
                command.Parameters.AddWithValue("@Category", animal.Category);
                command.Parameters.AddWithValue("@Area", animal.Area);
                connection.Open();
                int result = command.ExecuteNonQuery();

                return result > 0 ? NoContent() : NotFound();
            }
        }
        catch (SqlException ex)
        {
            return StatusCode(500, "error bazy danych: " + ex.Message);
        }
    }


    [HttpDelete("{idAnimal}")]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        var query = $"DELETE FROM Animal WHERE IdAnimal = @IdAnimal";

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                connection.Open();
                int result = command.ExecuteNonQuery();

                return result > 0 ? NoContent() : NotFound();
            }
        }
        catch (SqlException ex)
        {
            return StatusCode(500, "error bazy danych: " + ex.Message);
        }
    }

}