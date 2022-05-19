using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PokemonPocket
{
    class Skill
    {
        [Key]
        public string Name { get; set; }
        [ForeignKey("PType")]
        public string PTypeName { get; set; }
        public int BaseDamage { get; set; }
        public Skill(string name, int baseDamage, string pTypeName)
        {
            Name = name;
            BaseDamage = baseDamage;
            PTypeName = pTypeName;
        }
    }
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