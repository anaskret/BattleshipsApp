using Battleships.Models.Dtos;
using Battleships.Models.Entities.Example;
using Battleships.Models.Entities.Lobby;
using Battleships.Models.Entities.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.DataAccess
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Example> Examples { get; set; }
        public DbSet<Lobby> Lobbies { get; set; }
    }
}
