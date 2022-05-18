using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PokemonPocket
{
    class Pokemon
    {
        [Key]
        public int PokemonID { get; set; }
        [ForeignKey("Player")]
        public int PlayerID { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Pokemon()
        {
            Name = GetType().Name;
            Level = 1;
        }
        public static bool IsPokemon(string pokemonName)
        {
            Type possibleClass = Type.GetType($"PokemonPocket.{pokemonName}");
            return possibleClass.IsSubclassOf(typeof(Pokemon));
        }
    }

    class Pikachu : Pokemon
    {
        public Pikachu() : base()
        {
            Health = 20;
            MaxHealth = 20;
        }
    }

    class Raichu : Pikachu
    {
        public Raichu(bool evolved = false) : base()
        {
            MaxHealth = 30;
            if (evolved)
            {
                Health = 0;
            }
            else
            {
                Health = 30;
            }
        }
    }
}