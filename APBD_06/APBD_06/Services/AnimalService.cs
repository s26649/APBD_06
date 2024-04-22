using APBD_06.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace APBD_06.Services
{
    public class AnimalService
    {
        private readonly string _connectionString;
        private readonly List<string> _allowedSortFields = new List<string> { "name", "description", "category", "area" };

        public AnimalService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool IsValidOrderBy(string orderBy)
        {
            return _allowedSortFields.Contains(orderBy.ToLower());
        }
        
        public List<Animal> GetAnimals(string orderBy)
        {
            var animals = new List<Animal>();
            var query = $"SELECT IdAnimal, Name, Description, Category, Area FROM Animal ORDER BY {orderBy}";

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

            return animals;
        }

        public int AddAnimal(Animal animal)
        {
            var query = "INSERT INTO Animal (Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area)";
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", animal.Name);
                command.Parameters.AddWithValue("@Description", animal.Description);
                command.Parameters.AddWithValue("@Category", animal.Category);
                command.Parameters.AddWithValue("@Area", animal.Area);
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int UpdateAnimal(int idAnimal, Animal animal)
        {
            var query = $"UPDATE Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal";
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                command.Parameters.AddWithValue("@Name", animal.Name);
                command.Parameters.AddWithValue("@Description", animal.Description);
                command.Parameters.AddWithValue("@Category", animal.Category);
                command.Parameters.AddWithValue("@Area", animal.Area);
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int DeleteAnimal(int idAnimal)
        {
            var query = $"DELETE FROM Animal WHERE IdAnimal = @IdAnimal";
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
