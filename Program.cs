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
            game.Player.AddPokemon(game.PokemonContext);
            game.Player.AddPokemon(game.PokemonContext);
            game.Player.ShowPokemons();
            game.Player.EvolvePokemon(game.PokemonContext);
            // game.Player.RemovePokemon(game.PokemonContext);
            game.Player.ShowPokemons();
        }
    }
}
