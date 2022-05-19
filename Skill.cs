using System.ComponentModel.DataAnnotations;
namespace PokemonPocket
{
    class Skill
    {
        [Key]
        public string Name { get; set; }
        public int BaseDamage { get; set; }
        public Skill(string name, int baseDamage)
        {
            Name = name;
            BaseDamage = baseDamage;
        }
    }
}