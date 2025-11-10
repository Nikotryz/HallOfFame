using HallOfFame.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Data.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly HallOfFameDbContext _db;

    public PersonRepository(HallOfFameDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Person>> GetAllPersonsAsync()
    {
        return await _db.Persons.Include(p => p.Skills).ToListAsync();
    }

    public async Task<Person?> GetPersonByIdAsync(long id)
    {
        return await _db.Persons.Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Person> CreatePersonAsync(Person person)
    {
        await _db.Persons.AddAsync(person);
        await _db.SaveChangesAsync();
        return person;
    }

    public async Task<Person> UpdatePersonAsync(Person person)
    {
        _db.Persons.Update(person);
        await _db.SaveChangesAsync();
        return person;
    }

    public void DeleteSkills(IEnumerable<Skill> skills)
    {
        _db.Skills.RemoveRange(skills);
    }

    public async Task DeletePersonAsync(long id)
    {
        var person = await _db.Persons.FindAsync(id);
        if (person != null)
        {
            _db.Persons.Remove(person);
            await _db.SaveChangesAsync();
        }
    }
}