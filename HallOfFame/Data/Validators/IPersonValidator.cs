using HallOfFame.Data.DTOs;

namespace HallOfFame.Data.Validators;

/// <summary>
/// Общий интерфейс, отвечающий за валидацию входных данных из запроса.
/// </summary>
public interface IPersonValidator
{   
    ValidationResult Validate(PersonUpsertDto? personDto);
}