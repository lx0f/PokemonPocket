using System.ComponentModel.DataAnnotations;
namespace PokemonPocket
{
    class Player
    {
        [Key]
        public int PlayerID { get; set; }
        public string Name { get; private set; }

        public Player() { }
        public Player(string name) { Name = name; }
    }
}