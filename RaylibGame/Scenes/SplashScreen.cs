using Raylib_cs;
using RaylibGame.Engine;

namespace RaylibGame.Scenes {
    public class SplashScreen : IScene {
        private int _frame;
        private float _alpha;
        
        public ReturnActions Start() {
            Raylib.SetTargetFPS(60);
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Update() {
            _frame++;
            
            if (_frame > 60) {
                _alpha += 0.01f;
                if (_alpha >= 1.0f) {
                    Game.ChangeScene(new MainMenu());
                }
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE)) {
                Game.ChangeScene(new MainMenu());
            }
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Render() {
            Raylib.ClearBackground(Color.RAYWHITE);
            
            Raylib.DrawRectangle(Raylib.GetScreenWidth()/2 - 128, Raylib.GetScreenHeight()/2 - 128, 256, 256, Color.BLACK);
            Raylib.DrawRectangle(Raylib.GetScreenWidth()/2 - 112, Raylib.GetScreenHeight()/2 - 112, 224, 224, Color.RAYWHITE);
            Raylib.DrawText("raylib", Raylib.GetScreenWidth()/2 - 44, Raylib.GetScreenHeight()/2 + 48, 50, Color.BLACK);
            
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), new Color((byte)0, (byte)0, (byte)0, (byte)(255 * _alpha)));
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Close() {
            return ReturnActions.ReturnNull;
        }
    }
}