using System.ComponentModel.Design;
using System.Data.SqlClient;

namespace HtApiController;

public class Animal
{
    public int IdAnimal { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Area { get; set; }
    

    public Animal(string name, string description, string category, string area)
    {
        IdAnimal = getCounter();
        this.Name = name;
        this.Description = description;
        this.Category = category;
        this.Area = area;
    }

    public Animal()
    {
    }

    public override string ToString()
    {
        return $"IdAnimal: {IdAnimal}, Name: {Name}, Description: {Description}, Category: {Category}, Area: {Area}";
    }

    public int getCounter()
    {
        int counter = 1;
        string connection = "Server=localhost;Database=apbd;User Id=SA;Password=248652Alexey;";
        List<Animal> animals = GetAnimals(connection);
        foreach (var animal  in animals)
        {
            counter++;
        }
        
        return counter;
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
    
}