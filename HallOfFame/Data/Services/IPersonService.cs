using HallOfFame.Data.DTOs;

namespace HallOfFame.Data.Services;

/// <summary>
/// Общий интерфейс, отвечающий за основную бизнес-логику: взаимодействие с репозиторием, валидация и т.д.
/// </summary>
public interface IPersonService
{
    Task<IEnumerable<PersonResponseDto>> GetAllPersonsAsync();

    Task<PersonResponseDto> GetPersonByIdAsync(long id);

    Task<ServiceResult<PersonResponseDto>> CreatePersonAsync(PersonUpsertDto? person);

    Task<ServiceResult<PersonResponseDto>> UpdatePersonAsync(long id, PersonUpsertDto? person);

    Task DeletePersonAsync(long id);
}