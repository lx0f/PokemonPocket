using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PokemonPocket
{
    class PType
    {
        [Key]
        public string Name { get; set; }
        public PType(string name) { Name = name; }
    }

    class PTypeStrength
    {
        [Key]
        public int PTypeStrengthID { get; set; }
        [ForeignKey("PType")]
        public string Name { get; set; }
        [ForeignKey("PType")]
        public string StrongAgainst { get; set; }
        public decimal DamageMultiplier { get; set; }
        public PTypeStrength(string name, string strongAgainst, decimal damageMultiplier)
        {
            Name = name;
            StrongAgainst = strongAgainst;
            DamageMultiplier = damageMultiplier;
        }
    }

    class PTypeResistant
    {
        [Key]
        public int PTypeResistantID { get; set; }
        [ForeignKey("PType")]
        public string Name { get; set; }
        [ForeignKey("PType")]
        public string ResistantAgainst { get; set; }
        public decimal ResistMultiplier { get; set; }
        public PTypeResistant(string name, string resistantAgainst, decimal resistMultiplier)
        {
            Name = name;
            ResistantAgainst = resistantAgainst;
            ResistMultiplier = resistMultiplier;
        }
    }
}