using HallOfFame.Data.DTOs;
using HallOfFame.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace HallOfFame.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _personService;
        public PersonsController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersons()
        {
            var response = await _personService.GetAllPersonsAsync();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonById(long id)
        {
            var response = await _personService.GetPersonByIdAsync(id);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonAsync([FromBody] PersonUpsertDto? personDto)
        {
            var serviceResult = await _personService.CreatePersonAsync(personDto);

            PersonResponseDto createdPerson = serviceResult.Data;

            return CreatedAtAction(nameof(GetPersonById), new { id = createdPerson.Id }, createdPerson);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersonAsync(long id, [FromBody] PersonUpsertDto? personDto)
        {
            await _personService.UpdatePersonAsync(id, personDto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonAsync(long id)
        {
            await _personService.DeletePersonAsync(id);

            return NoContent();
        }
    }
}
