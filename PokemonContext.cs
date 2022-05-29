using Microsoft.EntityFrameworkCore;
namespace PokemonPocket
{
    public class PokemonContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        // SKILL RELATED
        public DbSet<SkillMap> SkillMaps { get; set; }
        public DbSet<Skill> Skills { get; set; }
        // TYPE RELATED
        public DbSet<PType> PTypes { get; set; }
        public DbSet<PTypeStrength> PTypeStrengths { get; set; }
        public DbSet<PTypeResistant> PTypeResistants { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data source = Pokemon.db");
        }
    }
}