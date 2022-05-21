using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace PokemonPocket
{
    class PType
    {
        [Key]
        public string Name { get; set; }
        public PType(string name) { Name = name; }

        public static List<PType> ReturnTypes()
        {
            List<string> typesAsString = new List<string>
            {
                "Normal",
                "Fire",
                "Water",
                "Grass",
                "Electric",
                "Ice",
                "Fighting",
                "Poison",
                "Ground",
                "Flying",
                "Psychic",
                "Bug",
                "Rock",
                "Ghost",
                "Dark",
                "Dragon",
                "Steel",
                "Fairy",
            };

            List<PType> pTypes = typesAsString
                .Select(type => new PType(type))
                .ToList();

            return pTypes;
        }
    }
}