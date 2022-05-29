using System;
using System.Collections.Generic;
using System.Linq;
namespace PokemonPocket
{
    class ComputerBattle
    {
        public Player Player { get; set; }
        public Pokemon Pokemon { get; set; }
        public Pokemon EnemyPokemon { get; set; }
        public PokemonContext PokemonContext { get; set; }
        public ComputerBattle(Player player, PokemonContext pokemonContext)
        {
            Player = player;
            PokemonContext = pokemonContext;
            PlayerChoosePokemon();
            EnemyPokemon = new Pikachu();
            EnemyPokemon.LoadSkills(pokemonContext);
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
        public Pokemon PlayerChoosePokemon()
        {
            List<Pokemon> allowed = Player.Pokemons.Where(p => p.Health > 0).ToList();
            List<string> options = allowed
                .Select(p => $"{p.Name}\n\tLVL {p.Level}\n\tHP {p.Health}/{p.MaxHealth}")
                .ToList();
            int index = Insero.PromptInt("Choose your pokemon", options);
            Pokemon = allowed[index];
            return Pokemon;
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
            // TODO: handle no weak or strong attacks
            // Find strength
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
                Console.WriteLine("It wasn't very effective...");
            }
            receiver.Health -= netDamage;
        }
        public void FightLoop()
        {
            bool forfeited = false;
            while ((Pokemon.Health > 0 && EnemyPokemon.Health > 0) && !forfeited)
            {
                ShowScene();
                Console.WriteLine("(1) Fight");
                Console.WriteLine("(2) Pokemon");
                Console.WriteLine("(3) Forfeit");
                int choice = Insero.PromptInt("Choose action", 1, 3);

                switch (choice)
                {
                    case 1:
                        List<string> options = Pokemon.Skills.Select(s => $"Name: {s.Name}\t{s.PTypeName}").ToList();
                        int skillChoice = Insero.PromptInt("Choose skill", options);

                        Skill skill = Pokemon.UseSkill(skillChoice);
                        PokemonAttacks(Pokemon, EnemyPokemon, skill);
                        if (EnemyPokemon.Health > 0)
                        {
                            skill = EnemyPokemon.UseSkill(0);
                            PokemonAttacks(EnemyPokemon, Pokemon, skill);
                        }
                        break;
                    case 2:
                        PlayerChoosePokemon();
                        break;
                    case 3:
                        Console.WriteLine("You forfeited!");
                        forfeited = true;
                        break;
                }
            }
            Player.TotalFights += 1;

            if (Pokemon.Health <= 0 || forfeited)
            {
                Console.WriteLine("You loss!!");
            }
            else if (EnemyPokemon.Health <= 0)
            {
                Console.WriteLine("You won!!!");
                Pokemon.Experience += CalculateExperienceYield(Pokemon, EnemyPokemon);
                Pokemon.TryLevelUp(PokemonContext);
                Player.Wins += 1;
                return;
            }
        }
    }
}