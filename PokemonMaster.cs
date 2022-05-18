using System.Collections.Generic;
namespace PokemonPocket
{
    class PokemonMaster
    {
        public string EvolveFrom { get; set; }
        public string EvolveTo { get; set; }
        public int EvolveCriteria { get; set; }

        public PokemonMaster(string evolveFrom, string evolveTo, int evolveCriteria)
        {
            EvolveFrom = evolveFrom;
            EvolveTo = evolveTo;
            EvolveCriteria = evolveCriteria;
        }

        public static List<PokemonMaster> PokemonMasters
        {
            get
            {
                return new List<PokemonMaster>(){
                    new PokemonMaster(
                        evolveFrom: "Pikachu",
                        evolveTo: "Raichu",
                        evolveCriteria: 2
                    ),
                };
            }
        }
    }
}