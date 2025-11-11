namespace HallOfFame.Data.DTOs;

/// <summary>
/// DTO для создания/обновления сотрудника. (Update/Insert --> Upsert).
/// </summary>
public class PersonUpsertDto
{
    public required string Name { get; set; }

    public required string DisplayName { get; set; }

    public required List<SkillDto> Skills { get; set; }
}