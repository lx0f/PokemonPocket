using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PokemonPocket
{
    class SkillMap
    {
        [Key]
        public int SkillMapID { get; set; }
        [ForeignKey("Skill")]
        public string SkillName { get; set; }
        [ForeignKey("Pokemon")]
        public string PokemonName { get; set; }
        public int LevelCriteria { get; set; }

        public SkillMap(string skillName, string pokemonName, int levelCriteria)
        {
            SkillName = skillName;
            PokemonName = pokemonName;
            LevelCriteria = levelCriteria;
        }
    }
}