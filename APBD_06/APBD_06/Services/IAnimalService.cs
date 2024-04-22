using APBD_06.Models;

namespace APBD_06.Services
{
    public interface IAnimalService
    {
        bool IsValidOrderBy(string orderBy);
        List<string> GetAllowedSortFields();
        List<Animal> GetAnimals(string orderBy);
        int AddAnimal(Animal animal);
        int UpdateAnimal(int idAnimal, Animal animal);
        int DeleteAnimal(int idAnimal);
    }

}