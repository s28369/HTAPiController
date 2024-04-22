using System.Data.SqlClient;

namespace HtApiController.Services.Animals;

public class AnimalService : IAnimalsService
{
    public async void CreateAnimal(Animal animal)
    {
        string connectionString = "Server=localhost;Database=apbd;User Id=SA;Password=248652Alexey;";
        string sqlInsert = "INSERT INTO Animal (IdAnimal, Name, Description, Category, Area) " +
                           "VALUES (@IdAnimal, @Name, @Description, @Category, @Area)";


        string IdAnimal = animal.IdAnimal.ToString();
        string name = animal.Name;
        string description = animal.Description;
        string category = animal.Category;
        string area = animal.Area;

        try
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();


                using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                {
                    SqlCommand commandIdentity = new SqlCommand("SET IDENTITY_INSERT Animal ON;", connection);

                    command.Parameters.AddWithValue("@IdAnimal", IdAnimal);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@Area", area);


                    commandIdentity.ExecuteNonQuery();
                    int rowsAffected = command.ExecuteNonQuery();
                    


                    Console.WriteLine($"Rows Affected: {rowsAffected}");
                }
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public Animal GetAnimal(int id)
    {
        string connectionString = "Server=localhost;Database=apbd;User Id=SA;Password=248652Alexey;";
        List<Animal> animals = GetAnimals(connectionString);

        foreach (Animal animal in animals)
        {
            if (animal.IdAnimal == id)
            {
                return animal;
            }
        }

        return new Animal();
    }
    List<Animal> GetAnimals(string connectionString)
    {
        List<Animal> animals = new List<Animal>();


        string sqlSelect = "SELECT IdAnimal, Name, Description, Category, Area FROM Animal";

        try
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();


                using (SqlCommand command = new SqlCommand(sqlSelect, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Animal animal = new Animal
                            {
                                IdAnimal = (int)reader["IdAnimal"],
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"] is DBNull ? null : reader["Description"].ToString(),
                                Category = reader["Category"].ToString(),
                                Area = reader["Area"].ToString()
                            };
                            
                            animals.Add(animal);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return animals;
    }

    public void DeleteAnimal(int id)
    {
        string connectionString = "Server=localhost;Database=apbd;User Id=SA;Password=248652Alexey;";
        List<Animal> animals = GetAnimals(connectionString);

        foreach (Animal animal in animals)
        {
            if (animal.IdAnimal == id)
            {
                

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string IdAnimal = id.ToString();
                        connection.Open();

                        string commandDelete = "DELETE FROM Animal WHERE IdAnimal IN (@idAnimal)";
                        using (SqlCommand command = new SqlCommand(commandDelete, connection))
                        {
                            command.Parameters.AddWithValue("@idAnimal", IdAnimal);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
        
    }

    public void UpsertAnimal(int id, Animal animal)
    {
        try
        {
            string connectionString = "Server=localhost;Database=apbd;User Id=SA;Password=248652Alexey;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string IdAnimal = id.ToString();
                string name = animal.Name;
                string description = animal.Description;
                string category = animal.Category;
                string area = animal.Area;
                
                connection.Open();

                string commandUpdate = "UPDATE Animal " +
                                       "SET Description = @description, Category = @category, Area = @area, Name = @name " +
                                       "WHERE idAnimal = @idAnimal;";
                using (SqlCommand command = new SqlCommand(commandUpdate, connection))
                {
                    command.Parameters.AddWithValue("@idAnimal", IdAnimal);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@category", category);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@area", area);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public List<Animal> GetAnimal(string type)
    {
        string connectionString = "Server=localhost;Database=apbd;User Id=SA;Password=248652Alexey;";
        List<Animal> animals = GetAnimals(connectionString);
        switch (type)
        {
            case "description": 
                return animals.OrderBy(a => a.Description).ToList();
            case "category": 
                return animals.OrderBy(a => a.Category).ToList();
            case "area": 
                return animals.OrderBy(a => a.Area).ToList();
            default: 
                return animals.OrderBy(a => a.Name).ToList();
        }
    }
}


