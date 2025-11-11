using HallOfFame.Data.Models;

namespace HallOfFame.Data.Repositories;

/// <summary>
/// Общий интерфейс, отвечающий за взаимодействие с БД и манипуляции с данными сотрудников.
/// </summary>
public interface IPersonRepository
{
    Task<IEnumerable<Person>> GetAllPersonsAsync();

    Task<Person?> GetPersonByIdAsync(long id);

    Task<Person> CreatePersonAsync(Person person);

    Task<Person> UpdatePersonAsync(Person person);

    void DeleteSkills(IEnumerable<Skill> skills);

    Task DeletePersonAsync(long id);
}