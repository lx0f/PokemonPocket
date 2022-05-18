using Microsoft.EntityFrameworkCore;
namespace PokemonPocket
{
    class PokemonContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data source = Pokemon.db");
        }
    }
}