using AutoMapper;
using HallOfFame.Data.DTOs;
using HallOfFame.Data.Models;
using HallOfFame.Data.Repositories;
using HallOfFame.Data.Services;
using HallOfFame.Exceptions;
using HallOfFame.Data.Validators;
using Moq;

namespace HallOfFame.Tests.UnitTests;

public class PersonServiceTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IPersonRepository> _mockRepo;
    private PersonService _service;

    public PersonServiceTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepo = new Mock<IPersonRepository>();
        _service = new PersonService(_mockMapper.Object, _mockRepo.Object, new PersonValidator());
    }

    [Fact]
    public async Task GetAllPersonsAsync_ReturnsPersonsList()
    {
        // Arrange
        var personsList = new List<Person>()
        {
            GetTestPerson(),
            GetTestPerson()
        };
        var personsResponseList = new List<PersonResponseDto>()
        {
            GetTestResponsePerson(),
            GetTestResponsePerson()
        };
        _mockMapper.Setup(m => m.Map<IEnumerable<PersonResponseDto>>(personsList)).Returns(personsResponseList);
        _mockRepo.Setup(repo => repo.GetAllPersonsAsync()).ReturnsAsync(personsList);

        // Act
        var result = await _service.GetAllPersonsAsync();

        // Assert
        Assert.Equal(personsList.Count, result.Count());
    }

    [Fact]
    public async Task GetAllPersonsAsync_ReturnsEmptyList_WhenNoPersons()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetAllPersonsAsync()).ReturnsAsync((ICollection<Person>)null);

        // Act
        var result = await _service.GetAllPersonsAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPersonByIdAsync_ReturnsPerson()
    {
        // Arrange
        var person = GetTestPerson();
        var upsertPerson = GetTestUpsertPerson();
        var responsePerson = GetTestResponsePerson();

        _mockMapper.Setup(m => m.Map<Person>(upsertPerson)).Returns(person);
        _mockMapper.Setup(m => m.Map<PersonResponseDto>(person)).Returns(responsePerson);
        _mockRepo.Setup(repo => repo.CreatePersonAsync(It.IsAny<Person>())).ReturnsAsync(person);

        var serviceResult = await _service.CreatePersonAsync(upsertPerson);
        PersonResponseDto createdPerson = serviceResult.Data;

        _mockRepo.Setup(repo => repo.GetPersonByIdAsync(createdPerson.Id)).ReturnsAsync(person);

        // Act
        var result = await _service.GetPersonByIdAsync(createdPerson.Id);

        // Assert
        Assert.Equal(createdPerson.Name, result.Name);
        Assert.Equal(createdPerson.DisplayName, result.DisplayName);
        _mockRepo.Verify(repo => repo.CreatePersonAsync(It.IsAny<Person>()), Times.Once);
    }

    [Fact]
    public async Task GetPersonByIdAsync_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetPersonByIdAsync(1)).ReturnsAsync((Person)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await _service.GetPersonByIdAsync(1);
        });
    }

    [Fact]
    public async Task CreatePersonAsync_ReturnsCorrectResult()
    {
        // Arrange
        var person = GetTestPerson();
        var upsertPerson = GetTestUpsertPerson();
        var responsePerson = GetTestResponsePerson();

        _mockMapper.Setup(m => m.Map<Person>(upsertPerson)).Returns(person);
        _mockMapper.Setup(m => m.Map<PersonResponseDto>(person)).Returns(responsePerson);
        _mockRepo.Setup(r => r.CreatePersonAsync(It.IsAny<Person>())).ReturnsAsync(person);

        // Act
        var serviceResult = await _service.CreatePersonAsync(upsertPerson);
        PersonResponseDto result = serviceResult.Data;

        // Assert
        Assert.Equal(responsePerson.Id, result.Id);
        Assert.Equal(responsePerson.Name, result.Name);
        Assert.Equal(responsePerson.DisplayName, result.DisplayName);
        _mockRepo.Verify(r => r.CreatePersonAsync(It.IsAny<Person>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePersonAsync_ReturnsUpdatedPerson_WhenPersonExists()
    {
        // Arrange
        var id = 1L;

        Person existingPerson = GetTestPerson();

        PersonUpsertDto updatePerson = new()
        {
            Name = "New Name",
            DisplayName = "New Display",
            Skills = new List<SkillDto>() { new SkillDto { Name = "Kotlin", Level = 4 } }
        };

        Person mappedPerson = new()
        {
            Name = "New Name",
            DisplayName = "New Display",
            Skills = new List<Skill>() { new Skill { Name = "Kotlin", Level = 4 } }
        };

        Person updatedPerson = new()
        {
            Id = id,
            Name = "New Name",
            DisplayName = "New Display",
            Skills = new List<Skill>() { new Skill { Name = "Kotlin", Level = 4 } }
        };

        PersonResponseDto expectedResponse = new()
        {
            Id = id,
            Name = "New Name",
            DisplayName = "New Display",
            Skills = new List<SkillDto>() { new SkillDto { Name = "Kotlin", Level = 4 } }
        };

        _mockRepo.Setup(repo => repo.GetPersonByIdAsync(id)).ReturnsAsync(existingPerson);

        _mockMapper.Setup(m => m.Map<Person>(updatePerson)).Returns(mappedPerson);

        _mockRepo.Setup(repo => repo.UpdatePersonAsync(It.IsAny<Person>())).ReturnsAsync(updatedPerson);

        _mockMapper.Setup(m => m.Map<PersonResponseDto>(updatedPerson)).Returns(expectedResponse);

        // Act
        var serviceResult = await _service.UpdatePersonAsync(id, updatePerson);
        PersonResponseDto result = serviceResult.Data;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Id, result.Id);
        Assert.Equal(expectedResponse.Name, result.Name);
        Assert.Equal(expectedResponse.DisplayName, result.DisplayName);

        _mockRepo.Verify(r => r.GetPersonByIdAsync(id), Times.Once);
        _mockRepo.Verify(r => r.UpdatePersonAsync(It.Is<Person>(p => p.Name == "New Name" && p.DisplayName == "New Display")), Times.Once);
    }

    [Fact]
    public async Task UpdatePersonAsync_ReturnsNull_WhenPersonNotExists()
    {
        var id = 1000000L;
        var upsertPerson = GetTestUpsertPerson();

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            var result = await _service.UpdatePersonAsync(id, upsertPerson);
        });
    }

    private Person GetTestPerson()
    {
        return new Person { Id = 1, Name = "Test", DisplayName = "T", Skills = [new Skill { Name = "C#", Level = 7 }] };
    }

    private PersonResponseDto GetTestResponsePerson()
    {
        return new PersonResponseDto { Id = 1, Name = "Test", DisplayName = "T", Skills = [ new SkillDto { Name = "C#", Level = 7}] };
    }

    private PersonUpsertDto GetTestUpsertPerson()
    {
        return new PersonUpsertDto { Name = "Test", DisplayName = "T", Skills = [new SkillDto { Name = "C#", Level = 7 }] };
    }
}