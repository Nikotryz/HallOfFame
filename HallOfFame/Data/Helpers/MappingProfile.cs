using AutoMapper;
using HallOfFame.Data.DTOs;
using HallOfFame.Data.Models;

namespace HallOfFame.Data.Helpers;

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