using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PokemonPocket
{
    class Skill
    {
        [Key]
        public string Name { get; set; }
        [ForeignKey("PType")]
        public string PTypeName { get; set; }
        public int BaseDamage { get; set; }
        public Skill(string name, int baseDamage, string pTypeName)
        {
            Name = name;
            BaseDamage = baseDamage;
            PTypeName = pTypeName;
        }

        public static List<Skill> ReturnSkills()
        {
            List<Skill> skills = new List<Skill>()  // yes i know.
            {
                new Skill("Acid",9,"Poison"),
                new Skill("Air Slash",14,"Flying"),
                new Skill("Astonish",8,"Ghost"),
                new Skill("Bite",6,"Dark"),
                new Skill("Bubble",12,"Water"),
                new Skill("Bug Bite",5,"Bug"),
                new Skill("Bullet Punch",9,"Steel"),
                new Skill("Bullet Seed",8,"Grass"),
                new Skill("Charge Beam",8,"Electric"),
                new Skill("Charm",20,"Fairy"),
                new Skill("Confusion",20,"Psychic"),
                new Skill("Counter",12,"Fighting"),
                new Skill("Cut",5,"Normal"),
                new Skill("Dragon Breath",6,"Dragon"),
                new Skill("Dragon Tail",15,"Dragon"),
                new Skill("Ember",10,"Fire"),
                new Skill("Extrasensory",12,"Psychic"),
                new Skill("Electro Ball",12,"Electric"),
                new Skill("Feint Attack",10,"Dark"),
                new Skill("Fire Fang",12,"Fire"),
                new Skill("Fire Spin",14,"Fire"),
                new Skill("Frost Breath",10,"Ice"),
                new Skill("Fury Cutter",3,"Bug"),
                new Skill("Gust",25,"Flying"),
                new Skill("Hex",10,"Ghost"),
                new Skill("Ice Fang",12,"Ice"),
                new Skill("Ice Shard",12,"Ice"),
                new Skill("Incinerate",29,"Fire"),
                new Skill("Infestation",10,"Bug"),
                new Skill("Iron Tail",15,"Steel"),
                new Skill("Karate Chop",8,"Fighting"),
                new Skill("Lick",5,"Ghost"),
                new Skill("Lock On",1,"Normal"),
                new Skill("Low Kick",6,"Fighting"),
                new Skill("Lightning Bolt",7,"Electric"),
                new Skill("Magical Leaf",16,"Grass"),
                new Skill("Metal Claw",8,"Steel"),
                new Skill("Mud Shot",5,"Ground"),
                new Skill("Mud Slap",18,"Ground"),
                new Skill("Peck",10,"Flying"),
                new Skill("Poison Jab",10,"Poison"),
                new Skill("Poison Sting",5,"Poison"),
                new Skill("Pound",7,"Normal"),
                new Skill("Powder Snow",6,"Ice"),
                new Skill("Present",5,"Normal"),
                new Skill("Psycho Cut",5,"Psychic"),
                new Skill("Quick Attack",8,"Normal"),
                new Skill("Razor Leaf",13,"Grass"),
                new Skill("Rock Smash",15,"Fighting"),
                new Skill("Rock Throw",12,"Rock"),
                new Skill("Scratch",6,"Normal"),
                new Skill("Shadow Claw",9,"Ghost"),
                new Skill("Smack Down",16,"Rock"),
                new Skill("Snarl",12,"Dark"),
                new Skill("Spark",10,"Electric"),
                new Skill("Splash",0,"Water"),
                new Skill("Steel Wing",11,"Steel"),
                new Skill("Struggle Bug",15,"Bug"),
                new Skill("Sucker Punch",7,"Dark"),
                new Skill("Tackle",5,"Normal"),
                new Skill("Take Down",8,"Normal"),
                new Skill("Thunder Fang",12,"Electric"),
                new Skill("Thunder Shock",5,"Electric"),
                new Skill("Thunder Wave",10,"Electric"),
                new Skill("Thunder Bolt",20,"Electric"),
                new Skill("Transform",0,"Normal"),
                new Skill("Vine Whip",7,"Grass"),
                new Skill("Volt Switch",14,"Electric"),
                new Skill("Water Gun",5,"Water"),
                new Skill("Water Gun Blastoise",10,"Water"),
                new Skill("Waterfall",16,"Water"),
                new Skill("Wing Attack",8,"Flying"),
                new Skill("Yawn",0,"Normal"),
                new Skill("Zen Headbutt",12,"Psychic"),
            };

            return skills;
        }
        public string Display()
        {
            return $"Name: {Name}\t\t{PTypeName}";
        }
    }
}