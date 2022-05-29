using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace PokemonPocket
{
    public class Server
    {
        public bool Forfeited { get; set; } = false;
        public Player Player { get; set; }
        public Pokemon Pokemon { get; set; }
        public Pokemon EnemyPokemon { get; set; }
        public BinaryWriter Writer { get; set; }
        public BinaryReader Reader { get; set; }
        public PokemonContext PokemonContext { get; set; }
        public Server(Player player, PokemonContext pokemonContext)
        {
            Player = player;
            PokemonContext = pokemonContext;
        }
        public void ShowScene()
        {
            int maxBars = 30;
            int playerBars = (int)(maxBars * (decimal)Pokemon.Health / (decimal)Pokemon.MaxHealth);
            int enemyBars = (int)(maxBars * (decimal)EnemyPokemon.Health / (decimal)EnemyPokemon.MaxHealth);
            Console.WriteLine($"{Pokemon.Name}");
            Console.WriteLine("[" + new String('#', playerBars) + new string(' ', maxBars - playerBars) + $"] {Pokemon.Health}/{Pokemon.MaxHealth}");
            Console.WriteLine();
            Console.WriteLine($"{EnemyPokemon.Name}");
            Console.WriteLine("[" + new String('#', enemyBars) + new string(' ', maxBars - enemyBars) + "]");
        }
        public int CalculateExperienceYield(Pokemon pokemon, Pokemon enemyPokemon)
        {
            decimal b = enemyPokemon.BaseExperience;
            decimal L = enemyPokemon.Level;
            decimal Lp = pokemon.Level;
            decimal exp = (decimal)Math.Pow(
                (double)((2 * L + 10) / (L + Lp + 10)), 2.5) * ((b * L) / 5);
            return (int)exp;
        }
        public void PokemonAttacks(Pokemon attacker, Pokemon receiver, Skill skill)
        {
            Writer.Write($"{attacker.Name} uses {skill.Name}!");
            Console.WriteLine($"{attacker.Name} uses {skill.Name}!");

            int netDamage = 0;
            decimal damageMultiplier = PokemonContext.PTypeStrengths
                .Where(strength => strength.Name == skill.PTypeName)
                .Where(strength => strength.StrongAgainst == receiver.PTypeName)
                .Select(strenth => strenth.DamageMultiplier)
                .FirstOrDefault();
            if (damageMultiplier > 0)
            {
                netDamage = (int)(damageMultiplier * skill.BaseDamage);
                Writer.Write("It was very effective!");
                Console.WriteLine("It was very effective!");
            }

            // Find resist
            decimal resistMultiplier = PokemonContext.PTypeResistants
                .Where(resist => resist.Name == receiver.PTypeName)
                .Where(resist => resist.ResistantAgainst == skill.PTypeName)
                .Select(resist => resist.ResistMultiplier)
                .FirstOrDefault();
            if (resistMultiplier > 0)
            {
                netDamage = (int)(resistMultiplier * skill.BaseDamage);
                Writer.Write("It wasn't very effective...");
                Console.WriteLine("It wasn't very effective...");
            }
            Writer.Write(netDamage);
            receiver.Health -= netDamage;
        }
        public void Send(int i)
        {
            List<string> options = null;
            switch (i)
            {
                case 1:  // FIGHT
                    options = Pokemon.Skills.Select(s => $"Name: {s.Name}\t{s.PTypeName}").ToList();
                    int skillChoice = Insero.PromptInt("Choose skill", options);

                    Skill skill = Pokemon.UseSkill(skillChoice);
                    Writer.Write("SKILL");
                    PokemonAttacks(Pokemon, EnemyPokemon, skill);
                    break;
                case 2:  // CHANGE POKMEON
                    List<Pokemon> allowed = Player.Pokemons.Where(p => p.Health > 0).ToList();
                    options = allowed.Select(p => $"{p.Name} Lv. {p.Level} Hp. {p.Health}/{p.MaxHealth}").ToList();
                    int choice = Insero.PromptInt("Choose a pokemon", options);
                    Pokemon = allowed[choice];
                    string pokemonJson = JsonConvert.SerializeObject(Pokemon);
                    Writer.Write("CHANGE");
                    Writer.Write(pokemonJson);
                    break;
                case 3: // FORFEIT
                    Writer.Write("FORFEIT");
                    Forfeited = true;
                    break;
            }
        }
        public void Recieve(string state)
        {
            Console.WriteLine("Other player choosing what to do...");
            switch (state)
            {
                case "CHANGE":
                    EnemyPokemon = JsonConvert.DeserializeObject<Pokemon>(Reader.ReadString());
                    Console.WriteLine("Other player changed pokemon!");
                    break;
                case "SKILL":
                    // Pokemon uses skill
                    Console.WriteLine(Reader.ReadString());
                    // It was / wasn't effective
                    Console.WriteLine(Reader.ReadString());
                    int netDamage = Reader.ReadInt32();
                    Pokemon.Health -= netDamage;
                    break;
                case "FORFEIT":
                    Console.WriteLine("Other player forfeited!");
                    EnemyPokemon.Health = 0;
                    break;
            }
        }
        public void Start()
        {
            TcpListener server = null;
            try
            {
                // Setup connection
                IPAddress addr = IPAddress.Parse("127.0.0.1");
                int port = 8000;
                server = new TcpListener(addr, port);
                server.Start();
                Console.WriteLine("Waiting for connection...");
                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                Reader = new BinaryReader(stream);
                Writer = new BinaryWriter(stream);
                Console.WriteLine("Connection established!");

                // Recieve enemy pokemon
                Console.WriteLine("Waiting for other player to select a pokemon...");
                EnemyPokemon = JsonConvert.DeserializeObject<Pokemon>(Reader.ReadString());

                // Select pokemon
                List<Pokemon> allowed = Player.Pokemons.Where(p => p.Health > 0).ToList();
                List<string> options = allowed.Select(p => $"{p.Name} Lv. {p.Level} Hp. {p.Health}/{p.MaxHealth}").ToList();
                int choice = Insero.PromptInt("Choose a pokemon", options);
                Pokemon = allowed[choice];
                string pokemonJson = JsonConvert.SerializeObject(Pokemon);
                Writer.Write(pokemonJson);

                // Fight loop
                while ((Pokemon.Health > 0 && EnemyPokemon.Health > 0) || Forfeited)  // WIN || LOSE
                {
                    // Show scene
                    Console.WriteLine("Other player choosing what to do...");
                    string state = Reader.ReadString();
                    Recieve(state);

                    ShowScene();
                    Console.WriteLine("(1) Fight");  // SKILL
                    Console.WriteLine("(2) Pokemon");  // CHANGE
                    Console.WriteLine("(3) Forfeit");  // FORFEIT
                    int fightChoice = Insero.PromptInt("Choose action", 1, 3);
                    Send(fightChoice);
                }

                server.Stop();
                stream.Close();
                Reader.Close();
                Writer.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                server.Stop();
            }

            Player.TotalFights += 1;
            if (Pokemon.Health <= 0 || Forfeited)
            {
                Console.WriteLine("You loss!!");
            }
            else if (EnemyPokemon.Health <= 0)
            {
                Console.WriteLine("You won!!!");
                Pokemon.Experience += CalculateExperienceYield(Pokemon, EnemyPokemon);
                Pokemon.TryLevelUp(PokemonContext);
                Player.Wins += 1;
            }
        }
    }
}