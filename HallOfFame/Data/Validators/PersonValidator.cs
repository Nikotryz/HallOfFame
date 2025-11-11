using HallOfFame.Data.DTOs;

namespace HallOfFame.Data.Validators;

/// <summary>
/// Реализация интерфейса <seealso cref="IPersonValidator"/>.
/// </summary>
public class PersonValidator : IPersonValidator
{
    public ValidationResult Validate(PersonUpsertDto? personDto)
    {
        if (personDto == null)
        {
            return ValidationResult.Failure("PersonDto is null");
        }
            
        return GeneralValidate(personDto.Name, personDto.DisplayName, personDto.Skills);
    }

    private static ValidationResult GeneralValidate(string name, string displayName, List<SkillDto> skills)
    {
        if (string.IsNullOrEmpty(name))
        {
            return ValidationResult.Failure("Name should not be empty or null.");
        }

        if (string.IsNullOrEmpty(displayName))
        {
            return ValidationResult.Failure("Display Name should not be empty or null.");
        }

        if (name.Length > 100)
        {
            return ValidationResult.Failure("Name should not be longer than 100 characters.");
        }

        if (displayName.Length > 100)
        {
            return ValidationResult.Failure("Display Name should not be longer than 100 characters.");
        }

        if (skills.Count == 0)
        {
            return ValidationResult.Failure("Person must have at least one skill.");
        }

        foreach (SkillDto skillDto in skills)
        {
            if (string.IsNullOrEmpty(skillDto.Name))
            {
                return ValidationResult.Failure($"Skill name should not be empty or null.");
            }

            if (skillDto.Name.Length > 100)
            {
                return ValidationResult.Failure($"Skill '{skillDto.Name}': Name should not be longer than 100 characters.");
            }
            
            if (skillDto.Level < 1 || skillDto.Level > 10)
            {
                return ValidationResult.Failure($"Skill '{skillDto.Name}': Level must be between 1 and 10.");
            }
        }

        return ValidationResult.Success();
    }
}