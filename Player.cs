using System.ComponentModel.DataAnnotations;
namespace PokemonPocket
{
    class Player
    {
        [Key]
        public int PlayerID { get; set; }
        public string Name { get; private set; }
        public string Summary { get { return $"Name: {Name}"; } }

        public Player() { }
        public Player(string name) { Name = name; }
        public void ChangeName(string newName) { Name = newName; }
    }
}