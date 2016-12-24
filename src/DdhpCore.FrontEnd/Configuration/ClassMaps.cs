using AutoMapper;
using DdhpCore.FrontEnd.Models.Api;

namespace DdhpCore.FrontEnd.Configuration
{
    public class ClassMaps
    {
        public static void BuildMaps(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Storage.Models.Club, Club>();
            cfg.CreateMap<Storage.Models.Fixture, Fixture>();
            cfg.CreateMap<Storage.Models.Contract, Contract>();
            cfg.CreateMap<Storage.Models.Player, Player>();
            cfg.CreateMap<Storage.Models.PlayedTeam, PlayedTeam>();
            cfg.CreateMap<Storage.Models.PlayedTeam.TeamPlayer, PlayedTeam.TeamPlayer>();
            cfg.CreateMap<Storage.Models.Stat, Stat>();
            cfg.CreateMap<Storage.Models.Round, Round>();
        }
    }
}