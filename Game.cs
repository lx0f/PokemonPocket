using System;
namespace PokemonPocket
{
    class Game
    {
        private PokemonContext PokemonContext { get; set; }
        public Game()
        {
            PokemonContext = new PokemonContext();
            PokemonContext.Database.EnsureCreated();
            Console.WriteLine("Loading assets");
            Console.WriteLine("Welcome to Pokemon Pocket!");
        }

        // TODO: Change to private
        public Player CreatePlayer()
        {
            Console.WriteLine("Enter your name");
            Console.Write(">>> ");
            string playerName = Console.ReadLine();
            Player newPlayer = new Player(playerName);
            PokemonContext.Players.Add(newPlayer);
            PokemonContext.SaveChanges();
            return newPlayer;
        }
    }
}