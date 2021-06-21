using AutoMapper;
using Battleships.Models.Dtos;
using Battleships.Models.Entities.Lobby;
using Battleships.Repositories.Repositories.Interfaces;
using Battleships.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Services.Services
{
    public class LobbyService : ILobbyService
    {
        private readonly ILobbyRepository _lobbyRepository;
        private readonly IMapper _mapper;

        public LobbyService(ILobbyRepository lobbyRepository, IMapper mapper)
        {
            _lobbyRepository = lobbyRepository;
            _mapper = mapper;
        }

        public async Task<LobbyDto> GetLobbyById(int id)
        {
            return _mapper.Map<LobbyDto>(await _lobbyRepository.GetByIdAsync(id));
        }

        public async Task<LobbyDto> CreateLobby(LobbyDto lobby)
        {
            var doesExist = await GetLobbyByName(lobby.Name);

            if (doesExist != null)
                throw new Exception("Lobby with that name already exists!");

            var createdLobby = await _lobbyRepository.AddAsync(_mapper.Map<Lobby>(lobby));
            return _mapper.Map<LobbyDto>(createdLobby);
        }

        public async Task UpdateLobby(LobbyDto lobby)
        {
            await _lobbyRepository.UpdateAsync(_mapper.Map<Lobby>(lobby));
        }

        public async Task DeleteLobby(int id)
        {
            var lobby = await _lobbyRepository.GetByIdAsync(id);
            await _lobbyRepository.DeleteAsync(lobby);
        }

        public async Task<List<LobbyDto>> GetAll()
        {
            var lobbies = await _lobbyRepository.GetAllAsync();
            return lobbies.Select(prp => _mapper.Map<LobbyDto>(prp)).ToList();
        }

        public async Task<LobbyDto> GetLobbyByName(string name)
        {
            return _mapper.Map<LobbyDto>(await _lobbyRepository.GetByNameAsync(name));
        }
    }
}
