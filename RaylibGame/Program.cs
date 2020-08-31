using Raylib_cs;
using RaylibGame.Engine;
using RaylibGame.Scenes;

namespace RaylibGame {
    internal static class Program {
        public static void Main(string[] args) {
            Game game = new Game();
            
            game.Start();
            
            while (!Raylib.WindowShouldClose()) {
                if (game.Update()) 
                    break;
                if (game.Render()) 
                    break;
            }
            
            game.Close();
            
            Raylib.CloseWindow();
        }
    }
}