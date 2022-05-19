using System;
using System.Linq;
using System.Collections.Generic;
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
        [ForeignKey("PType")]
        public string PTypeName { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        [NotMapped]
        public List<Skill> Skills { get; set; }
        public Pokemon()
        {
            Name = GetType().Name;
            Level = 1;
        }
        public List<Skill> LoadSkills(PokemonContext pokemonContext)
        {
            List<Skill> skills = new List<Skill>() { };
            pokemonContext.SkillMaps
                .Where(skillMap => skillMap.PokemonName == Name)
                .Where(skillMap => skillMap.LevelCriteria <= Level)
                .OrderBy(skillMap => skillMap.SkillName)
                .ToList()
                .ForEach(skillMap =>
                {
                    Skill skill = pokemonContext.Skills.Find(skillMap.SkillName);
                    skills.Add(skill);
                });
            Skills = skills;
            return skills;
        }
        // TODO
        public void TryLevelUp(PokemonContext pokemonContext)
        {
            ++Level;
            MaxHealth += 5;
            LoadSkills(pokemonContext);
        }
        public void ShowSkills()
        {
            for (int i = 0; i < Skills.Count; i++)
            {
                Skill skill = Skills[i];
                Console.WriteLine($"ID: {i} Name: {skill.Name} Base Damage: {skill.BaseDamage}");
            }
        }
        public Skill UseSkill(int index)
        {
            return Skills[index];
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
            MaxHealth = 20;
            Health = MaxHealth;
            PTypeName = "Electric";
        }
    }

    class Raichu : Pikachu
    {
        public Raichu(bool evolved = false) : base()
        {
            MaxHealth = 30;
            Health = evolved ? 0 : MaxHealth;
        }
    }
}