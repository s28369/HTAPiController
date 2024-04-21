namespace HtApiController.Services.Animals;

public interface IAnimalsService
{
    void CreateAnimal(Animal animal);
    Animal GetAnimal(int id);
    void DeleteAnimal(int id);
    void UpsertAnimal(int id, Animal animal);
    List<Animal> GetAnimal(string type);
} 