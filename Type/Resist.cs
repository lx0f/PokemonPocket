using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PokemonPocket
{
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

        public static List<PTypeResistant> ReturnTypeResists()
        {
            decimal resist = 0.8m;
            Dictionary<string, List<string>> resistDictionary = new Dictionary<string, List<string>>()
            {
                {"Normal", new List<string>(){
                    "Ghost"
                }},
                {"Fire", new List<string>(){
                    "Bug",
                    "Steel",
                    "Fire",
                    "Grass",
                    "Ice"
                }},
                {"Water", new List<string>(){
                    "Steel",
                    "Fire",
                    "Water",
                    "Ice"
                }},
                {"Grass", new List<string>(){
                    "Ground",
                    "Water",
                    "Grass",
                    "Electric"
                }},
                {"Electric", new List<string>(){
                    "Flying",
                    "Steel",
                    "Electric"
                }},
                {"Ice", new List<string>(){
                    "Ice"
                }},
                {"Fighting", new List<string>(){
                    "Rock",
                    "Bug",
                    "Dark"
                }},
                {"Poison", new List<string>(){
                    "Fighting",
                    "Poison",
                    "Grass",
                    "Fairy"
                }},
                {"Ground", new List<string>(){
                    "Poison",
                    "Rock",
                    "Electric"
                }},
                {"Flying", new List<string>(){
                    "Fighting",
                    "Bug",
                    "Grass",
                    "Ground"
                }},
                {"Psychic", new List<string>(){
                    "Fighting",
                    "Psychic"
                }},
                {"Bug", new List<string>(){
                    "Fighting",
                    "Ground",
                    "Grass"
                }},
                {"Rock", new List<string>(){
                    "Normal",
                    "Flying",
                    "Poison",
                    "Fire"
                }},
                {"Ghost", new List<string>(){
                    "Poison",
                    "Bug",
                    "Normal",
                    "Fighting"
                }},
                {"Dark", new List<string>(){
                    "Ghost",
                    "Dark",
                    "Psychic"
                }},
                {"Dragon", new List<string>(){
                    "Fire",
                    "Water",
                    "Grass",
                    "Electric"
                }},
                {"Steel", new List<string>(){
                    "Normal",
                    "Flying",
                    "Rock",
                    "Bug",
                    "Steel",
                    "Grass",
                    "Psychic",
                    "Ice",
                    "Dragon",
                    "Fairy",
                    "Poison"
                }},
                {"Fairy", new List<string>(){
                    "Fighting",
                    "Bug",
                    "Dark",
                    "Dragon"
                }},
            };

            List<PTypeResistant> pTypeResistants = new List<PTypeResistant>();

            foreach (KeyValuePair<string, List<string>> item in resistDictionary)
            {
                foreach (string type in item.Value)
                {
                    pTypeResistants.Add(new PTypeResistant(item.Key, type, resist));
                }
            }

            return pTypeResistants;
        }
    }
}