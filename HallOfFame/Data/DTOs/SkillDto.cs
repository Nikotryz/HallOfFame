namespace HallOfFame.Data.DTOs;


/// <summary>
/// DTO для создания, обновления, а также отправки данных о навыках сотрудника.
/// </summary>
public class SkillDto
{
    public required string Name { get; set; }

    public required byte Level { get; set; }
}