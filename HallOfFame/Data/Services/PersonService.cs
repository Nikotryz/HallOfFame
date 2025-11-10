using AutoMapper;
using HallOfFame.Exceptions;
using HallOfFame.Data.DTOs;
using HallOfFame.Data.Models;
using HallOfFame.Data.Repositories;
using HallOfFame.Data.Validators;

namespace HallOfFame.Data.Services;

public class PersonService : IPersonService
{
    private readonly IMapper _mapper;
    private readonly IPersonRepository _personRepository;
    private readonly IPersonValidator _validator;

    public PersonService(IMapper mapper, IPersonRepository personRepository, IPersonValidator validator)
    {
        _mapper = mapper;
        _personRepository = personRepository;
        _validator = validator;
    }

    public async Task<IEnumerable<PersonResponseDto>> GetAllPersonsAsync()
    {
        var persons = await _personRepository.GetAllPersonsAsync();
        return _mapper.Map<IEnumerable<PersonResponseDto>>(persons);
    }

    public async Task<PersonResponseDto> GetPersonByIdAsync(long id)
    {
        var person = await _personRepository.GetPersonByIdAsync(id);

        if (person == null)
        {
            throw new NotFoundException($"Person with id '{id}' does not exist");
        }

        return _mapper.Map<PersonResponseDto>(person);
    }

    public async Task<ServiceResult<PersonResponseDto>> CreatePersonAsync(PersonUpsertDto? personDto)
    {
        var validationResult = _validator.Validate(personDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.ErrorMessage);
        }
        
        var person = _mapper.Map<Person>(personDto);
        await _personRepository.CreatePersonAsync(person);

        var result = _mapper.Map<PersonResponseDto>(person);
        return ServiceResult<PersonResponseDto>.Success(result);
    }

    public async Task<ServiceResult<PersonResponseDto>> UpdatePersonAsync(long id, PersonUpsertDto? personDto)
    {
        var validationResult = _validator.Validate(personDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.ErrorMessage);
        }
        
        var existingPerson = await _personRepository.GetPersonByIdAsync(id);
        if (existingPerson == null)
        {
            throw new NotFoundException($"Person with id '{id}' does not exist");
        }

        _personRepository.DeleteSkills(existingPerson.Skills);

        var person = _mapper.Map<Person>(personDto);

        existingPerson.Name = person!.Name;
        existingPerson.DisplayName = person.DisplayName;
        existingPerson.Skills = person.Skills.ToList();

        var updatePerson = await _personRepository.UpdatePersonAsync(existingPerson);

        var result = _mapper.Map<PersonResponseDto>(updatePerson);
        return ServiceResult<PersonResponseDto>.Success(result);
    }

    public async Task DeletePersonAsync(long id)
    {
        await _personRepository.DeletePersonAsync(id);
    }
}