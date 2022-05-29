using System;
using System.Linq;
using System.Collections.Generic;
namespace PokemonPocket
{
    public class Game
    {
        public PokemonContext PokemonContext { get; set; }
        // TODO: change to private
        public Player Player { get; set; }
        public Game()
        {
            PokemonContext = new PokemonContext();
            // PokemonContext.Database.EnsureDeleted();
            PokemonContext.Database.EnsureCreated();
            Console.WriteLine("Building game assets");
            BuildGameAssets();
            Console.WriteLine("Welcome to Pokemon Pocket!");
        }

        public void BuildGameAssets()
        {
            PokemonContext.Skills.RemoveRange(PokemonContext.Skills.ToList());
            PokemonContext.SkillMaps.RemoveRange(PokemonContext.SkillMaps.ToList());
            PokemonContext.PTypes.RemoveRange(PokemonContext.PTypes.ToList());
            PokemonContext.PTypeStrengths.RemoveRange(PokemonContext.PTypeStrengths.ToList());
            PokemonContext.PTypeResistants.RemoveRange(PokemonContext.PTypeResistants.ToList());

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
            List<string> existingPlayerNames = PokemonContext.Players.Select(p => p.Name).ToList();
            string playerName = Insero.PromptString("Enter your new player name", existingPlayerNames);
            Player newPlayer = new Player(playerName);
            PokemonContext.Players.Add(newPlayer);
            PokemonContext.SaveChanges();
            Console.WriteLine($"Created new player {playerName}!");
            Player = newPlayer;
            return newPlayer;
        }

        public Player LoadPlayer()
        {
            List<string> existingPlayerNames = PokemonContext.Players.Select(p => p.Name).ToList();
            string playerName = Insero.PromptString(existingPlayerNames, "Enter player name");
            Player existingPlayer = PokemonContext.Players
                .SingleOrDefault(player => player.Name == playerName);
            Player = existingPlayer;
            return existingPlayer;
        }

        public void Exit()
        {
            Console.WriteLine("Bye bye!!!");
            PokemonContext.SaveChanges();
            Environment.Exit(0);
        }
        public void GameLoop()
        {
            List<string> options = new List<string>(){
                "Create new player",
                "Select player"
            };
            int playerChoice = Insero.PromptInt("Choose", options);
            switch (playerChoice)
            {
                case 0:
                    AddPlayer();
                    break;
                case 1:
                    if (PokemonContext.Players.Count() == 0)
                    {
                        Console.WriteLine("There are no players to select from");
                        AddPlayer();
                        break;
                    }
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

                int gameChoice = Insero.PromptInt("Choose", 1, 9);

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
                        if (Player.Pokemons.Where(p => p.Health > 0).Count() == 0)
                        {
                            Console.WriteLine("You don't have any pokemons to fight with!");
                            break;
                        }
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
                    case 9:
                        List<string> netOptions = new List<string>() { "Host", "Connect" };
                        int choice = Insero.PromptInt("Choose", netOptions);
                        if (choice == 0)
                        {
                            Server server = new Server(Player, PokemonContext);
                            server.Pokemon = Player.Pokemons[0];
                            server.Start();
                            PokemonContext.SaveChanges();
                        }
                        else if (choice == 1)
                        {
                            Client client = new Client(Player, PokemonContext);
                            client.Start();
                            PokemonContext.SaveChanges();
                        }
                        break;
                }
            }
        }
    }
}