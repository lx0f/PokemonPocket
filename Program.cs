using System;

namespace PokemonPocket
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.AddPlayer();
            game.LoadPlayer();
            game.UpdatePlayer();
            game.RemovePlayer();
            game.ShowPlayerInfo();
        }
    }
}
