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
                return $"Name:\t{Name}\nFights:\t{TotalFights}\nWins:\t{Wins}\nPoke:\t{Pokemons.Count}";
            }
        }
        [NotMapped]
        public List<Pokemon> Pokemons { get; set; }

        public Player() { }
        public Player(string name) { Name = name; }
        public Pokemon AddPokemon(PokemonContext pokemonContext)
        {
            string pokemonName = Insero.PromptString("Enter pokemon name").ToLower();
            pokemonName = char.ToUpper(pokemonName[0]) + pokemonName.Substring(1);
            while (!(Pokemon.IsPokemon(pokemonName)))
            {
                Console.WriteLine("That is not a valid pokemon");
                pokemonName = Insero.PromptString("Enter pokemon name").ToLower();
                pokemonName = char.ToUpper(pokemonName[0]) + pokemonName.Substring(1);
            }
            Pokemon newPokemon = Pokemon.CreatePokemon(pokemonName);
            newPokemon.PlayerID = PlayerID;
            pokemonContext.Pokemons.Add(newPokemon);
            pokemonContext.SaveChanges();
            LoadPokemons(pokemonContext);
            return newPokemon;
        }
        public Pokemon RemovePokemon(PokemonContext pokemonContext)
        {
            List<string> options = Pokemons.Select(p => $"{p.Name} Lv. {p.Level} Hp. {p.Health}/{p.MaxHealth}").ToList();
            int index = Insero.PromptInt("Choose a pokemon to remove", options);
            Pokemon chosenPokemon = Pokemons[index];
            pokemonContext.Pokemons.Remove(chosenPokemon);
            pokemonContext.SaveChanges();
            LoadPokemons(pokemonContext);
            return chosenPokemon;
        }
        public Pokemon EvolvePokemon(PokemonContext pokemonContext)
        {
            if (!(ShowEvolvablePokemons()))
            {
                return null;
            }
            string pokemonName = Insero.PromptString("Enter pokemon name").ToLower();
            pokemonName = char.ToUpper(pokemonName[0]) + pokemonName.Substring(1);
            while (!(Pokemon.IsPokemon(pokemonName)))
            {
                Console.WriteLine("That pokemon does not exist!");
                pokemonName = Insero.PromptString("Enter pokemon name").ToLower();
                pokemonName = char.ToUpper(pokemonName[0]) + pokemonName.Substring(1);
            }
            Console.WriteLine("Enter pokemon name you want to evolve");
            Console.Write(">>> ");
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
                    List<string> options = toDeletePokemons.Select(p => $"{p.Name} Lv. {p.Level} Hp. {p.Health}/{p.MaxHealth}").ToList();
                    int index = Insero.PromptInt($"Choose which {pokemonName} to sacrifice, {deleteCriteria} left", options);
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
            playerPokemons.ForEach(pokemon => pokemon.LoadSkills(pokemonContext));
            Pokemons = playerPokemons;
            return playerPokemons;
        }
        public void ShowPokemons()
        {
            Pokemons.ForEach(pokemon =>
            {
                Console.WriteLine($"Name: {pokemon.Name}");
                Console.WriteLine($"Type: {pokemon.PTypeName}");
                Console.WriteLine($"Level: {pokemon.Level}");
                Console.WriteLine($"Health: {pokemon.Health}/{pokemon.MaxHealth}");
                Console.WriteLine($"Experience: {pokemon.Experience}/{pokemon.MaxExperience}");
                Console.WriteLine("Skills:");
                pokemon.Skills.ForEach(s => Console.WriteLine($"  - Name: {s.Name}  {s.PTypeName}"));
            });
        }
        public bool ShowEvolvablePokemons()
        {
            bool canEvolve = false;
            PokemonMaster.PokemonMasters.ForEach(pokemonMaster =>
            {
                int pokemonCount = Pokemons
                    .Where(pokemon => pokemon.Name == pokemonMaster.EvolveFrom)
                    .Count();

                if (pokemonCount >= pokemonMaster.EvolveCriteria)
                {
                    Console.WriteLine($"{pokemonMaster.EvolveFrom} --> {pokemonMaster.EvolveTo}!!");
                    canEvolve = true;
                }
            });

            if (!(canEvolve))
            {
                Console.WriteLine("There are no pokemons that can be evolved");
                return canEvolve;
            }
            return canEvolve;
        }
        public void HealAllPokemons()
        {
            Pokemons.ForEach(pokemon => pokemon.Health = pokemon.MaxHealth);
        }
    }
}