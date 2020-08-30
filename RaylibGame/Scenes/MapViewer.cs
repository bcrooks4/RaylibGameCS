using System.Numerics;
using Raylib_cs;
using RaylibGame.Engine;

namespace RaylibGame.Scenes {
    public class MapViewer : IScene {
        private Texture2D _mapTexture;
        private Camera2D _camera;
        
        public ReturnActions Start() {
            _mapTexture = Raylib.LoadTexture("export.png");
            _camera = new Camera2D {zoom = 1};
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Update() {
            _camera.offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);

            if (Raylib.GetMouseWheelMove() > 0) {
                _camera.zoom += 0.2f;
            }
            else if (Raylib.GetMouseWheelMove() < 0) {
                _camera.zoom -= 0.2f;
                if (_camera.zoom <= 0.05f) _camera.zoom = 0.05f;
            }

            if (Raylib.IsKeyDown(KeyboardKey.KEY_UP)) {
                _camera.target.Y -= 10;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN)) {
                _camera.target.Y += 10;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT)) {
                _camera.target.X -= 10;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT)) {
                _camera.target.X += 10;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE)) {
                Game.ChangeScene(new MapDrawing());
            }
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Render() {
            Raylib.BeginMode2D(_camera);
            Raylib.DrawTexture(_mapTexture, -_mapTexture.width / 2, -_mapTexture.height / 2, Color.WHITE);
            Raylib.EndMode2D();
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Close() {
            Raylib.UnloadTexture(_mapTexture);
            return ReturnActions.ReturnNull;
        }
    }
}