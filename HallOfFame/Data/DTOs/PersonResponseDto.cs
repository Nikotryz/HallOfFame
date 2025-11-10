namespace HallOfFame.Data.DTOs;
public class PersonResponseDto
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public List<SkillDto> Skills { get; set; } = null!;
}