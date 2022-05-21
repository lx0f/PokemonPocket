namespace PokemonPocket
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.GameLoop();
            // game.AddPlayer();
            // game.LoadPlayer();
            // game.BuildGameAssets();
            // game.Player.AddPokemon(game.PokemonContext);
            // game.Player.Pokemons.ForEach(pokemon =>
            // {
            //     pokemon.LoadSkills(game.PokemonContext);
            //     pokemon.ShowSkills();
            // });
            // game.Player.AddPokemon(game.PokemonContext);
            // game.Player.ShowPokemons();
            // ComputerBattle battle = new ComputerBattle(game.Player, game.PokemonContext);
            // battle.FightLoop();
            // game.Player.ShowEvolvablePokemons();
            // game.Player.EvolvePokemon(game.PokemonContext);
            // // game.Player.RemovePokemon(game.PokemonContext);
            // game.Player.ShowPokemons();
            // game.ShowPlayerInfo();
        }
    }
}
