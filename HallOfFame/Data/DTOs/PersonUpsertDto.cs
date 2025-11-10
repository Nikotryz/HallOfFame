namespace HallOfFame.Data.DTOs;

public class PersonUpsertDto
{
    public required string Name { get; set; }

    public required string DisplayName { get; set; }

    public required List<SkillDto> Skills { get; set; }
}