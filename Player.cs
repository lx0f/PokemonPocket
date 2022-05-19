using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PokemonPocket
{
    class Player
    {
        [Key]
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public int TotalFights { get; set; }
        public int Wins { get; set; }
        public string Summary
        {
            get
            {
                return @$"Name: {Name}
                Total Fights: {TotalFights}
                Wins: {Wins}";
            }
        }
        [NotMapped]
        public List<Pokemon> Pokemons { get; set; }

        public Player() { }
        public Player(string name) { Name = name; }
        public Pokemon AddPokemon(PokemonContext pokemonContext)
        {
            Console.WriteLine("Enter pokemon name");
            Console.Write(">>> ");
            string pokemonName = Console.ReadLine();

            Type pokemonClass = Type.GetType($"PokemonPocket.{pokemonName}");
            if (pokemonClass == null)
            {
                Console.WriteLine($"There is no pokemons with the name {pokemonName}");
                return null;
            }
            else
            {
                Pokemon newPokemon = (Pokemon)Activator.CreateInstance(pokemonClass);
                newPokemon.PlayerID = PlayerID;
                pokemonContext.Pokemons.Add(newPokemon);
                pokemonContext.SaveChanges();
                LoadPokemons(pokemonContext);
                return newPokemon;
            }
        }
        public Pokemon RemovePokemon(PokemonContext pokemonContext)
        {
            for (int i = 0; i < Pokemons.Count; i++)
            {
                Pokemon pokemon = Pokemons[i];
                Console.WriteLine($"{i}: {pokemon.Name} Lv. {pokemon.Level} Hp. {pokemon.Health}/{pokemon.MaxHealth}");
            }
            Console.WriteLine("Choose which pokemon to remove");
            Console.Write(">>> ");
            int index = Int32.Parse(Console.ReadLine());
            Pokemon chosenPokemon = Pokemons[index];
            pokemonContext.Pokemons.Remove(chosenPokemon);
            pokemonContext.SaveChanges();
            LoadPokemons(pokemonContext);
            return chosenPokemon;
        }
        public Pokemon EvolvePokemon(PokemonContext pokemonContext)
        {
            Console.WriteLine("Enter pokemon name you want to evolve");
            Console.Write(">>> ");
            string pokemonName = Console.ReadLine();

            Type pokemonClass = Type.GetType($"PokemonPocket.{pokemonName}");
            if (!Pokemon.IsPokemon(pokemonName)) { return null; }
            PokemonMaster pokemonMaster = PokemonMaster.PokemonMasters
                .SingleOrDefault(pokemonMaster => pokemonMaster.EvolveFrom == pokemonName);

            int pokemonCount = Pokemons
                .Where(pokemon => pokemon.Name == pokemonName)
                .Count();

            if (pokemonCount >= pokemonMaster.EvolveCriteria)
            {
                int deleteCriteria = pokemonMaster.EvolveCriteria;
                List<Pokemon> toDeletePokemons = Pokemons
                    .Where(pokemon => pokemon.Name == pokemonName)
                    .OrderBy(pokemon => pokemon.MaxHealth)
                    // .Take(pokemonMaster.EvolveCriteria)
                    .ToList();

                // Lets user select which pokemon to sacrifice
                List<Pokemon> confirmDeletePokemon = new List<Pokemon>() { };
                while (deleteCriteria > 0)
                {
                    Console.WriteLine($"Choose which {pokemonName} to sacrifice, {deleteCriteria} left");
                    for (int i = 0; i < toDeletePokemons.Count; i++)
                    {
                        Pokemon pokemon = toDeletePokemons[i];
                        Console.WriteLine($"{i}: {pokemon.Name} Lv. {pokemon.Level} Hp. {pokemon.Health}/{pokemon.MaxHealth}");
                    }
                    Console.Write(">>> ");
                    int index = Int32.Parse(Console.ReadLine());
                    Pokemon selectedPokemon = toDeletePokemons[index];
                    toDeletePokemons.Remove(selectedPokemon);
                    confirmDeletePokemon.Add(selectedPokemon);
                    --deleteCriteria;
                }

                pokemonContext.Pokemons.RemoveRange(confirmDeletePokemon);

                Type evolvedPokemonClass = Type.GetType($"PokemonPocket.{pokemonMaster.EvolveTo}");
                Pokemon newPokemon = (Pokemon)Activator.CreateInstance(evolvedPokemonClass, args: true);
                newPokemon.PlayerID = PlayerID;
                pokemonContext.Pokemons.Add(newPokemon);
                pokemonContext.SaveChanges();
                LoadPokemons(pokemonContext);
                return newPokemon;
            }
            else
            {
                Console.WriteLine($"{pokemonName} does not meet the evolve criteria");
                return null;
            }
        }
        public List<Pokemon> LoadPokemons(PokemonContext pokemonContext)
        {
            List<Pokemon> playerPokemons = pokemonContext.Pokemons
                .Where(pokemon => pokemon.PlayerID == PlayerID)
                .ToList();
            Pokemons = playerPokemons;
            return playerPokemons;
        }
        public void ShowPokemons()
        {
            Pokemons.ForEach(pokemon =>
            {
                Console.WriteLine($"Name: {pokemon.Name}");
                Console.WriteLine($"Level: {pokemon.Level}");
                Console.WriteLine($"Health: {pokemon.Health}");
                Console.WriteLine($"Max Health: {pokemon.MaxHealth}");
            });
        }
        public void ShowEvolvablePokemons()
        {
            PokemonMaster.PokemonMasters.ForEach(pokemonMaster =>
            {
                int pokemonCount = Pokemons
                    .Where(pokemon => pokemon.Name == pokemonMaster.EvolveFrom)
                    .Count();

                if (pokemonCount >= pokemonMaster.EvolveCriteria)
                {
                    Console.WriteLine($"{pokemonMaster.EvolveFrom} can be evolved into {pokemonMaster.EvolveTo}!!");
                }
            });
        }
        public void HealAllPokemons()
        {
            Pokemons.ForEach(pokemon =>
            {
                pokemon.Health = pokemon.MaxHealth;
            });
        }
    }
}