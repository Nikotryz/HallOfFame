namespace HallOfFame.Data.DTOs;

/// <summary>
/// DTO для предоставления данных сотрудника в виде ответа на запрос.
/// </summary>
public class PersonResponseDto
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public List<SkillDto> Skills { get; set; } = null!;
}