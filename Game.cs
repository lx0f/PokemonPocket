using System;
using System.Linq;
using System.Collections.Generic;
namespace PokemonPocket
{
    class Game
    {
        public PokemonContext PokemonContext { get; set; }
        // TODO: change to private
        public Player Player { get; set; }
        public Game()
        {
            PokemonContext = new PokemonContext();
            PokemonContext.Database.EnsureDeleted();
            PokemonContext.Database.EnsureCreated();
            Console.WriteLine("Loading assets");
            Console.WriteLine("Welcome to Pokemon Pocket!");
        }

        public void BuildGameAssets()
        {
            PokemonContext.Skills.RemoveRange(PokemonContext.Skills.ToList());
            PokemonContext.SkillMaps.RemoveRange(PokemonContext.SkillMaps.ToList());

            Console.WriteLine("Loading Types...");
            List<PType> pTypes = new List<PType>() {
                new PType("Electric"),
                new PType("Fire"),
                new PType("Normal"),
            };
            PokemonContext.PTypes.AddRange(pTypes);

            Console.WriteLine("Loading Type Strengths...");
            List<PTypeStrength> pTypeStrengths = new List<PTypeStrength>()
            {
                // TODO: Add more types
            };
            PokemonContext.PTypeStrengths.AddRange(pTypeStrengths);

            Console.WriteLine("Loading Type Resistants...");
            List<PTypeResistant> pTypeResistants = new List<PTypeResistant>()
            {
                // TODO: Add more types
                new PTypeResistant("Fire", "Fire", 0.8m),
                new PTypeResistant("Electric", "Electric", 0.8m),
            };
            PokemonContext.PTypeResistants.AddRange(pTypeResistants);

            Console.WriteLine("Loading Skills...");
            List<Skill> skills = new List<Skill>() {
                new Skill("Lightning Bolt", 5, "Electric"),
                new Skill("Solar Power", 5, "Fire"),
                new Skill("Run Away", 0, "Normal"),
            };
            PokemonContext.Skills.AddRange(skills);

            Console.WriteLine("Loading Skill Mapping...");
            List<SkillMap> skillMaps = new List<SkillMap>() {
                new SkillMap("Lightning Bolt", "Pikachu", 1),
                new SkillMap("Solar Power", "Charmander", 1),
                new SkillMap("Run Away", "Eevee", 1)
            };
            PokemonContext.SkillMaps.AddRange(skillMaps);

            PokemonContext.SaveChanges();
        }

        // TODO: Change to private
        public Player AddPlayer()
        {
            Console.WriteLine("Enter your new player name");
            Console.Write(">>> ");
            string playerName = Console.ReadLine();

            Player existingPlayer = PokemonContext.Players
                .SingleOrDefault(player => player.Name == playerName);
            if (existingPlayer == null)
            {
                Player newPlayer = new Player(playerName);
                PokemonContext.Players.Add(newPlayer);
                PokemonContext.SaveChanges();
                Console.WriteLine($"Created new player {playerName}!");
                Player = existingPlayer;
                return newPlayer;
            }
            else
            {
                Console.WriteLine($"A player with the name {playerName} already exists!");
                return null;
            }
        }

        public Player RemovePlayer()
        {
            Console.WriteLine("Enter name of player to delete");
            Console.Write(">>> ");
            string playerName = Console.ReadLine();

            Player existingPlayer = PokemonContext.Players
                .SingleOrDefault(player => player.Name == playerName);
            if (existingPlayer == null)
            {
                Console.WriteLine($"There are no players with the name {playerName}");
                return null;
            }
            else
            {
                PokemonContext.Players.Remove(existingPlayer);
                PokemonContext.SaveChanges();
                return existingPlayer;
            }
        }

        public Player LoadPlayer()
        {
            Console.WriteLine("Choose player");
            PokemonContext.Players
                .ToList()
                .ForEach(player => Console.WriteLine(player.Name));
            Console.Write(">>> ");
            string playerName = Console.ReadLine();

            Player existingPlayer = PokemonContext.Players
                .SingleOrDefault(player => player.Name == playerName);
            if (existingPlayer == null)
            {
                Console.WriteLine($"There is no player with the name {playerName}");
                return null;
            }
            else
            {
                Player = existingPlayer;
                return existingPlayer;
            }
        }

        // Only if Player is loaded
        public Player UpdatePlayer()
        {
            Console.WriteLine("Enter new player name replacement");
            Console.Write(">>> ");
            string newPlayerName = Console.ReadLine();
            if (newPlayerName == Player.Name)
            {
                Console.WriteLine("The name you entered is the same as your existing one.");
            }
            else if (PokemonContext.Players
                .SingleOrDefault(player => player.Name == newPlayerName) != null)
            {
                Console.WriteLine("Another player with this name exists!");
            }
            else
            {
                Player.Name = newPlayerName;
                PokemonContext.SaveChanges();
            }
            return Player;
        }

        public void ShowPlayerInfo()
        {
            Console.WriteLine(Player.Summary);
        }

        public void Exit()
        {
            Console.WriteLine("Bye bye!!!");
            Environment.Exit(0);
        }

        public void GameLoop()
        {
            BuildGameAssets();
            Console.WriteLine("1. Create new player");
            Console.WriteLine("2. Select player");
            Console.Write(">>> ");
            int playerChoice = Int32.Parse(Console.ReadLine());
            switch (playerChoice)
            {
                case 1:
                    AddPlayer();
                    LoadPlayer();
                    break;
                case 2:
                    LoadPlayer();
                    break;
            }
            Player.LoadPokemons(PokemonContext);
            while (true)
            {
                Console.WriteLine("1. Add pokemon");
                Console.WriteLine("2. List pokemons");
                Console.WriteLine("3. List evolvable pokemons");
                Console.WriteLine("4. Evolve pokemon");
                Console.WriteLine("5. Battle");
                Console.WriteLine("6. Exit");

                Console.Write(">>> ");
                int gameChoice = Int32.Parse(Console.ReadLine());

                switch (gameChoice)
                {
                    case 1:
                        Player.AddPokemon(PokemonContext);
                        break;
                    case 2:
                        Player.ShowPokemons();
                        break;
                    case 3:
                        Player.ShowEvolvablePokemons();
                        break;
                    case 4:
                        Player.EvolvePokemon(PokemonContext);
                        break;
                    case 5:
                        ComputerBattle computerBattle = new ComputerBattle(Player, PokemonContext);
                        computerBattle.FightLoop();
                        break;
                    case 6:
                        Exit();
                        break;
                }
            }
        }
    }
}