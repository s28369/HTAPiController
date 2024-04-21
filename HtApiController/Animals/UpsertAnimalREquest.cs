namespace HtApiController.Animals;

public record UpsertAnimalRequest(int idAnimal, string name, string description, string category, string area);