using System.Data.SqlClient;

namespace HtApiController.Services.Animals;

public class AnimalService : IAnimalsService
{
    public async void CreateAnimal(Animal animal)
    {
        string connectionString = "Server=localhost;Database=apbd;User Id=SA;Password=248652Alexey;";
        string sqlInsert = "INSERT INTO Animal (IdAnimal, Name, Description, Category, Area) " +
                           "VALUES (@IdAnimal, @Name, @Description, @Category, @Area)";

        // Sample values for insertion
        string IdAnimal = animal.IdAnimal.ToString();
        string name = animal.Name;
        string description = animal.Description;
        string category = animal.Category;
        string area = animal.Area;

        try
        {
            // Establish connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a command object
                using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                {
                    SqlCommand commandIdentity = new SqlCommand("SET IDENTITY_INSERT Animal ON;", connection);
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@IdAnimal", IdAnimal);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@Area", area);

                    // Execute the command
                    commandIdentity.ExecuteNonQuery();
                    int rowsAffected = command.ExecuteNonQuery();
                    

                    // Output the result
                    Console.WriteLine($"Rows Affected: {rowsAffected}");
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public Animal GetAnimal(int id)
    {
        string connectionString = "Server=localhost;Database=apbd;User Id=SA;Password=248652Alexey;";
        List<Animal> animals = GetAnimals(connectionString);

        // Display the retrieved animal data
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

        // SQL query to select all animals from the Animal table
        string sqlSelect = "SELECT IdAnimal, Name, Description, Category, Area FROM Animal";

        try
        {
            // Establish connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a command object
                using (SqlCommand command = new SqlCommand(sqlSelect, connection))
                {
                    // Execute the command and obtain a data reader
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Read each row from the data reader
                        while (reader.Read())
                        {
                            // Create an Animal object and populate it with data from the reader
                            Animal animal = new Animal
                            {
                                IdAnimal = (int)reader["IdAnimal"],
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"] is DBNull ? null : reader["Description"].ToString(),
                                Category = reader["Category"].ToString(),
                                Area = reader["Area"].ToString()
                            };

                            // Add the animal to the list
                            animals.Add(animal);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return animals;
    }

    public void DeleteAnimal(int id)
    {
        string connectionString = "Server=localhost;Database=apbd;User Id=SA;Password=248652Alexey;";
        List<Animal> animals = GetAnimals(connectionString);

        // Display the retrieved animal data
        foreach (Animal animal in animals)
        {
            if (animal.IdAnimal == id)
            {
                

                try
                {
                    // Establish connection to the database
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
                    // Handle any exceptions
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
            // Establish connection to the database
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
            // Handle any exceptions
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


