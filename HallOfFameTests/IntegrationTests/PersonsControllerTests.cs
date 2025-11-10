using HallOfFame.Data.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace HallOfFame.Tests.IntegrationTests;

public class PersonsControllerTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public PersonsControllerTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAllPersons()
    {
        var response = await _httpClient.GetAsync("api/v1/persons");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetPersonById_WhenPersonExists()
    {
        var person = new PersonUpsertDto
        {
            Name = "test",
            DisplayName = "t",
            Skills = new List<SkillDto> { new() { Name = "C#", Level = 7 } }
        };

        var createResponse = await _httpClient.PostAsJsonAsync("/api/v1/persons", person);
        var createdPerson = await createResponse.Content.ReadFromJsonAsync<PersonResponseDto>();
        var response = await _httpClient.GetAsync($"/api/v1/persons/{createdPerson!.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var fetchedPerson = await response.Content.ReadFromJsonAsync<PersonResponseDto>();

        Assert.Equal("test", fetchedPerson?.Name);
    }

    [Fact]
    public async Task GetPersonById_WhenPersonDoesNotExist()
    {
        var response = await _httpClient.GetAsync("/api/v1/persons/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreatePerson_WhenDataIsValid()
    {
        var person = new PersonUpsertDto
        {
            Name = "Valid data",
            DisplayName = "vd",
            Skills = new List<SkillDto>
            {
                new() { Name = "C#", Level = 7 },
                new() { Name = "Kotlin", Level = 4 }
            }
        };

        var response = await _httpClient.PostAsJsonAsync("/api/v1/persons", person);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdPerson = await response.Content.ReadFromJsonAsync<PersonResponseDto>();

        Assert.NotNull(createdPerson);
        Assert.Equal("Valid data", createdPerson.Name);
    }

    [Fact]
    public async Task CreatePerson_WhenDataIsInvalid()
    {
        var person = new PersonUpsertDto
        {
            Name = "",
            DisplayName = "diplay name",
            Skills = new List<SkillDto>
            {
                new() { Name = "C#", Level = 7 },
                new() { Name = "Kotlin", Level = 4 }
            }
        };

        var response = await _httpClient.PostAsJsonAsync("/api/v1/persons", person);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreatePerson_WhenSkillsIsEmpty()
    {
        var person = new PersonUpsertDto
        {
            Name = "Test",
            DisplayName = "t",
            Skills = new List<SkillDto>()
        };

        var response = await _httpClient.PostAsJsonAsync("/api/v1/persons", person);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePerson_WhenDataIsValid()
    {
        var createPerson = new PersonUpsertDto
        {
            Name = "Test Name",
            DisplayName = "Test",
            Skills = new List<SkillDto> { new() { Name = "C#", Level = 7 } }
        };

        var createResponse = await _httpClient.PostAsJsonAsync("/api/v1/persons", createPerson);
        var createdPerson = await createResponse.Content.ReadFromJsonAsync<PersonResponseDto>();

        var updatePerson = new PersonUpsertDto
        {
            Name = "New Name",
            DisplayName = "New",
            Skills = new List<SkillDto>
            {
                new() { Name = "Kotlin", Level = 4 },
                new() { Name = "Docker", Level = 7 }
            }
        };

        var response = await _httpClient.PutAsJsonAsync($"/api/v1/persons/{createdPerson!.Id}", updatePerson);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _httpClient.GetAsync($"/api/v1/persons/{createdPerson.Id}");
        var updatedPerson = await getResponse.Content.ReadFromJsonAsync<PersonResponseDto>();

        Assert.Equal("New Name", updatedPerson?.Name);
        Assert.Equal("Kotlin", updatedPerson?.Skills[0].Name);
        Assert.Equal("Docker", updatedPerson?.Skills[1].Name);
    }

    [Fact]
    public async Task UpdatePerson_WhenPersonDoesNotExist()
    {
        var updatePerson = new PersonUpsertDto
        {
            Name = "New Name",
            DisplayName = "New",
            Skills = new List<SkillDto>
            {
                new() { Name = "C#", Level = 7 },
                new() { Name = "Kotlin", Level = 4 }
            }
        };

        var response = await _httpClient.PutAsJsonAsync("/api/v1/persons/1000000", updatePerson);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeletePerson_WhenPersonExists()
    {
        var person = new PersonUpsertDto
        {
            Name = "Delete",
            DisplayName = "Delete",
            Skills = new List<SkillDto>
            {
                new() { Name = "Test", Level = 1 }
            }
        };

        var createResponse = await _httpClient.PostAsJsonAsync("/api/v1/persons", person);
        var createdPerson = await createResponse.Content.ReadFromJsonAsync<PersonResponseDto>();

        var responseDeletePerson = await _httpClient.DeleteAsync($"/api/v1/persons/{createdPerson!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, responseDeletePerson.StatusCode);

        var responseGetPerson = await _httpClient.GetAsync($"/api/v1/persons/{createdPerson.Id}");

        Assert.Equal(HttpStatusCode.NotFound, responseGetPerson.StatusCode);
    }

    [Fact]
    public async Task DeletePerson_WhenPersonDoesNotExist()
    {
        var response = await _httpClient.DeleteAsync("/api/v1/persons/1000000");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}