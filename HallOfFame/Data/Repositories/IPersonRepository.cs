using HallOfFame.Data.Models;

namespace HallOfFame.Data.Repositories;

public interface IPersonRepository
{
    Task<IEnumerable<Person>> GetAllPersonsAsync();

    Task<Person?> GetPersonByIdAsync(long id);

    Task<Person> CreatePersonAsync(Person person);

    Task<Person> UpdatePersonAsync(Person person);

    void DeleteSkills(IEnumerable<Skill> skills);

    Task DeletePersonAsync(long id);
}