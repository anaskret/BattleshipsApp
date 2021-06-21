using AutoMapper;
using Battleships.Models.Dtos;
using Battleships.Models.Entities.Player;
using Battleships.Repositories.Repositories.Interfaces;
using Battleships.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Services.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;

        public PlayerService(IPlayerRepository playerRepository, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _mapper = mapper;
        }

        public async Task<PlayerDto> CreatePlayer(PlayerDto player)
        {
            var doesExist = await GetPlayerByName(player.Username);

            if (doesExist != null)
                throw new Exception("Player with that name already exists!");

            var createdPlayer = await _playerRepository.AddAsync(_mapper.Map<Player>(player));
            return _mapper.Map<PlayerDto>(createdPlayer);
        }

        public async Task DeletePlayer(int id)
        {
            var player = await _playerRepository.GetByIdAsync(id);
            await _playerRepository.DeleteAsync(player);
        }

        public async Task<List<PlayerDto>> GetAll()
        {
            var players = await _playerRepository.GetAllAsync();
            return players.Select(prp => _mapper.Map<PlayerDto>(prp)).ToList();
        }

        public async Task<PlayerDto> GetPlayerByName(string name)
        {
            return _mapper.Map<PlayerDto>(await _playerRepository.GetByNameAsync(name));
        }

        public async Task UpdatePlayer(string name)
        {
            var player = await _playerRepository.GetByNameAsync(name);
            player.Wins++;

            await _playerRepository.UpdateAsync(_mapper.Map<Player>(player));
        }

        public async Task<List<PlayerDto>> GetRanking()
        {
            var players = await _playerRepository.GetAllAsync();
            return players.Select(prp => _mapper.Map<PlayerDto>(prp)).OrderBy(prp => prp.Wins).ToList();
        }
    }
}
