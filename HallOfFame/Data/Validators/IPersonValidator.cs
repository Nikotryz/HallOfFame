using HallOfFame.Data.DTOs;

namespace HallOfFame.Data.Validators;

public interface IPersonValidator
{   
    ValidationResult Validate(PersonUpsertDto? personDto);
}