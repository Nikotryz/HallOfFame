using AutoMapper;
using HallOfFame.Data.DTOs;
using HallOfFame.Data.Models;

namespace HallOfFame.Data.Helpers;

/// <summary>
/// AutoMapper упрощает преобразование объектов одних классов к другим.
/// MappingProfile определяет, какие классы к каким будут иметь возможность преобразоваться.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Person, PersonResponseDto>();
        CreateMap<Skill, SkillDto>();

        CreateMap<PersonUpsertDto, Person>();
        CreateMap<SkillDto, Skill>();
    }
}