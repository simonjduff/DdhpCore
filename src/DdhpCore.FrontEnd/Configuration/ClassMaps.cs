using AutoMapper;
using DdhpCore.FrontEnd.Models.Api;
using DdhpCore.FrontEnd.Models.Api.Read;
using DdhpCore.FrontEnd.Models.Storage;

namespace DdhpCore.FrontEnd.Configuration
{
    public class ClassMaps
    {
        public static void BuildMaps(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Club, ClubApi>()
                .ForMember(dest => dest.ClubName, opt => opt.MapFrom(source => source.Details.ClubName))
                .ForMember(dest => dest.CoachName, opt => opt.MapFrom(source => source.Details.CoachName));
            cfg.CreateMap<ClubSeason, ClubSeasonApi>()
                .ForMember(dest => dest.ClubId, opt => opt.MapFrom(source => source.Id));
            cfg.CreateMap<Contract, ClubSeasonApi.Contract>();
        }
    }
}