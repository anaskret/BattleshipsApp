using AutoMapper;
using Battleships.Models.Dtos;
using Battleships.Repositories.Repositories.Interfaces;
using Battleships.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Services.Services
{
    public class ExampleService : IExampleService
    {
        private readonly IExampleRepository _exampleRepository;
        private readonly IMapper _mapper;

        public ExampleService(IExampleRepository exampleRepository, IMapper mapper)
        {
            _exampleRepository = exampleRepository;
            _mapper = mapper;
        }

        public async Task<List<ExampleDto>> GetExampleList()
        {
            var examples = await _exampleRepository.GetAllAsync();
            return examples.Select(prp => _mapper.Map<ExampleDto>(prp)).ToList();
        }
    }
}
