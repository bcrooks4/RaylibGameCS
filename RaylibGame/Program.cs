using Raylib_cs;
using RaylibGame.Engine;
using RaylibGame.Scenes;

namespace RaylibGame {
    internal class Program {
        public static void Main(string[] args) {

            Game game = new Game();
            
            game.Start();
            
            while (!Raylib.WindowShouldClose()) {
                game.Update();
                game.Render();
            }
            
            game.Close();
            
            Raylib.CloseWindow();
        }
    }
}