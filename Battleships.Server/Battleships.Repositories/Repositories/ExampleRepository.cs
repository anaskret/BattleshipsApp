using Battleships.DataAccess;
using Battleships.Models.Entities.Example;
using Battleships.Repositories.Repositories.Generic;
using Battleships.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Repositories.Repositories
{
    public class ExampleRepository : Repository<Example>, IExampleRepository
    {
        public ExampleRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
