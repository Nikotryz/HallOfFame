using HallOfFame.Data.DTOs;

namespace HallOfFame.Data.Services;

public interface IPersonService
{
    Task<IEnumerable<PersonResponseDto>> GetAllPersonsAsync();

    Task<PersonResponseDto> GetPersonByIdAsync(long id);

    Task<ServiceResult<PersonResponseDto>> CreatePersonAsync(PersonUpsertDto? person);

    Task<ServiceResult<PersonResponseDto>> UpdatePersonAsync(long id, PersonUpsertDto? person);

    Task DeletePersonAsync(long id);
}