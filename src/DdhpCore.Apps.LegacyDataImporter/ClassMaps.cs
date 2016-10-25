using System;
using AutoMapper;
using LegacyDataImporter.LegacyModels;
using LegacyDataImporter.Models;
using Player = LegacyDataImporter.LegacyModels.Player;
using Round = LegacyDataImporter.LegacyModels.Round;

namespace LegacyDataImporter
{
    public class ClassMaps
    {
        public static void BuildMaps(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Round, Models.Round>();
            cfg.CreateMap<Team, Club>()
                .ConvertUsing(team => new Club
                {
                    LegacyId = team.Id,
                    CoachName = team.CoachName,
                    ClubName = team.TeamName,
                    Email = team.Email,
                    Id = Guid.NewGuid()
                });
            cfg.CreateMap<Player, Models.Player>()
                .ConvertUsing(player =>
                {
                    var middleNames = string.IsNullOrWhiteSpace(player.MiddleNames)
                        ? String.Empty : $" {player.MiddleNames}";
                    return new Models.Player
                    {
                        Name = $"{player.FirstName}{middleNames} {player.LastName}",
                        Id = Guid.NewGuid(),
                        Active = player.Active,
                        CurrentAflClub = new Models.Player.AflClub
                        {
                            Name = player.CurrentAflTeam.Name,
                            ShortName = player.CurrentAflTeam.ShortName
                        },
                        FootywireName = player.FootywireName
                    };
                });
        }
    }
}