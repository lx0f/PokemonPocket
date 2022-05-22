using System.Collections.Generic;
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

        public static List<SkillMap> ReturnSkillMaps()
        {
            List<SkillMap> skillMaps = new List<SkillMap>()
            {
                new SkillMap("Lightning Bolt", "Pikachu", 1),
                new SkillMap("Thunder Shock", "Pikachu", 2),
                new SkillMap("Thunder Wave", "Pikachu", 5),
                new SkillMap("Thunder Bolt", "Pikachu", 10),
            };

            return skillMaps;
        }
    }
}