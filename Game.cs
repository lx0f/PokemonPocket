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
            Console.WriteLine("Building game assets");
            BuildGameAssets();
            Console.WriteLine("Welcome to Pokemon Pocket!");
        }

        public void BuildGameAssets()
        {
            PokemonContext.Skills.RemoveRange(PokemonContext.Skills.ToList());
            PokemonContext.SkillMaps.RemoveRange(PokemonContext.SkillMaps.ToList());

            Console.WriteLine("Loading Types...");
            List<PType> pTypes = PType.ReturnTypes();
            PokemonContext.PTypes.AddRange(pTypes);

            Console.WriteLine("Loading Type Strengths...");
            List<PTypeStrength> pTypeStrengths = PTypeStrength.ReturnTypeStrengths();
            PokemonContext.PTypeStrengths.AddRange(pTypeStrengths);

            Console.WriteLine("Loading Type Resistants...");
            List<PTypeResistant> pTypeResistants = PTypeResistant.ReturnTypeResists();
            PokemonContext.PTypeResistants.AddRange(pTypeResistants);

            Console.WriteLine("Loading Skills...");
            List<Skill> skills = Skill.ReturnSkills();
            PokemonContext.Skills.AddRange(skills);

            Console.WriteLine("Loading Skill Mapping...");
            List<SkillMap> skillMaps = SkillMap.ReturnSkillMaps();
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
                Player = newPlayer;
                return newPlayer;
            }
            else
            {
                Console.WriteLine($"A player with the name {playerName} already exists!");
                return null;
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

        public void Exit()
        {
            Console.WriteLine("Bye bye!!!");
            PokemonContext.SaveChanges();
            Environment.Exit(0);
        }
        public void GameLoop()
        {
            Console.WriteLine("(1) Create new player");
            Console.WriteLine("(2) Select player");
            Console.Write(">>> ");
            int playerChoice = Int32.Parse(Console.ReadLine());
            switch (playerChoice)
            {
                case 1:
                    AddPlayer();
                    break;
                case 2:
                    LoadPlayer();
                    break;
            }
            Player.LoadPokemons(PokemonContext);
            while (true)
            {
                Console.WriteLine("(1) Add pokemon");
                Console.WriteLine("(2) List pokemons");
                Console.WriteLine("(3) List evolvable pokemons");
                Console.WriteLine("(4) Evolve pokemon");
                Console.WriteLine("(5) Battle");
                Console.WriteLine("(6) Heal pokemons");
                Console.WriteLine("(7) Show player info");
                Console.WriteLine("(8) Exit");

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
                        Player.HealAllPokemons();
                        break;
                    case 7:
                        Console.WriteLine(Player.Summary);
                        break;
                    case 8:
                        Exit();
                        break;
                }
            }
        }
    }
}