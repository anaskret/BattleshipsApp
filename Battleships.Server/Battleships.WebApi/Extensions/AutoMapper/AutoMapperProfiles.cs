using AutoMapper;
using Battleships.Models.Dtos;
using Battleships.Models.Entities.Example;
using Battleships.Models.Entities.Lobby;
using Battleships.Models.Entities.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.WebApi.Extensions.AutoMapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<Lobby, LobbyDto>();
            CreateMap<LobbyDto, Lobby>();
            CreateMap<Player, PlayerDto>();
            CreateMap<PlayerDto, Player>();
        }
    }
}
