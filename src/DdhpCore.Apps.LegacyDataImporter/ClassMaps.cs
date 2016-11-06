using System;
using System.Collections.Generic;
using AutoMapper;
using LegacyDataImporter.LegacyModels;
using LegacyDataImporter.Models;
using Contract = LegacyDataImporter.LegacyModels.Contract;
using Fixture = LegacyDataImporter.LegacyModels.Fixture;
using Player = LegacyDataImporter.LegacyModels.Player;
using Round = LegacyDataImporter.LegacyModels.Round;
using Stat = LegacyDataImporter.LegacyModels.Stat;

namespace LegacyDataImporter
{
    public class ClassMaps
    {
        public static void BuildMaps(IMapperConfigurationExpression cfg)
        {
            var playerIdMaps = new Dictionary<int, Guid>();
            var clubIdMaps = new Dictionary<int, Guid>();
            var aflClubMaps = new Dictionary<int, Guid>();

            cfg.CreateMap<Round, Models.Round>();
            cfg.CreateMap<Team, Club>()
                .ConvertUsing(team =>
                {
                    var clubId = Guid.NewGuid();

                    if (!clubIdMaps.ContainsKey(team.Id))
                    {
                        clubIdMaps.Add(team.Id, clubId);
                    }

                    return new Club
                    {
                        LegacyId = team.Id,
                        CoachName = team.CoachName,
                        ClubName = team.TeamName,
                        Email = team.Email,
                        Id = clubId
                    };
                });
            cfg.CreateMap<Player, Models.Player>()
                .ConvertUsing(player =>
                {
                    var middleNames = string.IsNullOrWhiteSpace(player.MiddleNames)
                        ? String.Empty : $" {player.MiddleNames}";
                    var playerId = Guid.NewGuid();

                    if (!playerIdMaps.ContainsKey(player.Id))
                    {
                        playerIdMaps.Add(player.Id, playerId);
                    }

                    return new Models.Player
                    {
                        Name = $"{player.FirstName}{middleNames} {player.LastName}",
                        Id = playerId,
                        Active = player.Active,
                        CurrentAflClubId = aflClubMaps[player.CurrentAflTeamId],
                        FootywireName = player.FootywireName,
                        LegacyId = player.Id
                    };
                });
            cfg.CreateMap<Contract, Models.Contract>()
                .ConvertUsing(contract =>
                    new Models.Contract
                    {
                        ToRound = contract.ToRound,
                        FromRound = contract.FromRound,
                        DraftPick = contract.DraftPick,
                        PlayerId = playerIdMaps[contract.PlayerId],
                        ClubId = clubIdMaps[contract.TeamId]
                    });
            cfg.CreateMap<Fixture, Models.Fixture>()
                .ConvertUsing(fixture => new Models.Fixture
                {
                    RoundId = fixture.Round,
                    Home = clubIdMaps[fixture.HomeTeamId],
                    Away = clubIdMaps[fixture.AwayTeamId]
                });
            cfg.CreateMap<AflTeam, AflClub>()
                .ConvertUsing(team =>
                {
                    var newId = Guid.NewGuid();
                    aflClubMaps.Add(team.Id, newId);
                    return new AflClub
                    {
                        Id = newId,
                        ShortName = team.ShortName,
                        Name = team.Name
                    };
                });
            cfg.CreateMap<Stat, Models.Stat>()
                .ForMember(stat => stat.AflClubId, config => config.MapFrom(oldStat => aflClubMaps[oldStat.AFLTeamId]))
                .ForMember(stat => stat.PlayerId, config => config.MapFrom(oldStat => playerIdMaps[oldStat.PlayerId]));
        }
    }
}