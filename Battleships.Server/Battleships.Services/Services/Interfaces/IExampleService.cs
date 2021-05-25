using Battleships.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Battleships.Services.Services.Interfaces
{
    public interface IExampleService
    {
        Task<List<ExampleDto>> GetExampleList();
    }
}
