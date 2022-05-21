using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonPocket
{
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

        public static List<PTypeStrength> ReturnTypeStrengths()
        {
            decimal strength = 2m;
            Dictionary<string, List<string>> strengthDictionary = new Dictionary<string, List<string>>()
            {
                {"Normal", new List<string>(){

                }},
                {"Fire", new List<string>(){
                    "Bug",
                    "Steel",
                    "Grass",
                    "Ice",
                }},
                {"Water", new List<string>(){
                    "Ground",
                    "Rock",
                    "Fire",
                }},
                {"Grass", new List<string>(){
                    "Ground",
                    "Rock",
                    "Water",
                }},
                {"Electric", new List<string>(){
                    "Flying",
                    "Water",
                }},
                {"Ice", new List<string>(){
                    "Flying",
                    "Ground",
                    "Grass",
                    "Dragon",
                }},
                {"Fighting", new List<string>(){
                    "Normal",
                    "Rock",
                    "Steel",
                    "Ice",
                    "Dark",
                }},
                {"Poison", new List<string>(){
                    "Grass",
                    "Fairy",
                }},
                {"Ground", new List<string>(){
                    "Poison",
                    "Rock",
                    "Steel",
                    "Fire",
                    "Electric",
                }},
                {"Flying", new List<string>(){
                    "Fighting",
                    "Bug",
                    "Grass",
                }},
                {"Psychic", new List<string>(){
                    "Fighting",
                    "Poison",
                }},
                {"Bug", new List<string>(){
                    "Grass",
                    "Psychic",
                    "Dark",
                }},
                {"Rock", new List<string>(){
                    "Flying",
                    "Bug",
                    "Fire",
                    "Ice",
                }},
                {"Ghost", new List<string>(){
                    "Ghost",
                    "Psychic",
                }},
                {"Dark", new List<string>(){
                    "Ghost",
                    "Psychic",
                }},
                {"Dragon", new List<string>(){
                    "Dragon",
                }},
                {"Steel", new List<string>(){
                    "Rock",
                    "Ice",
                    "Fairy",
                }},
                {"Fairy", new List<string>(){
                    "Fighting",
                    "Dragon",
                    "Dark",
                }},
            };

            List<PTypeStrength> pTypeStrengths = new List<PTypeStrength>();

            foreach (KeyValuePair<string, List<string>> item in strengthDictionary)
            {
                foreach (string type in item.Value)
                {
                    pTypeStrengths.Add(new PTypeStrength(item.Key, type, strength));
                }
            }

            return pTypeStrengths;
        }
    }
}