using System.Drawing;
using System.Numerics;
using Raylib_cs;
using RaylibGame.Engine;
using RaylibGame.Types;
using Color = Raylib_cs.Color;

namespace RaylibGame.Scenes {
    public class RegionViewer : IScene {
        private int _width = 0;
        private int _height = 0;
        private RegionType _regionType = 0;
        private Color _colourLight = Color.MAGENTA;
        private Color _colourDark = Color.GRAY;
        
        private Camera2D _camera;

        private MapViewer _mapViewer;

        public RegionViewer(int width, int height, RegionType regionType, MapViewer mapViewer) {
            _width = width;
            _height = height;
            _regionType = regionType;
            _camera = new Camera2D {zoom = 1};
            _mapViewer = mapViewer;

            switch (regionType) {
                case RegionType.Ocean:
                    _colourLight = Color.SKYBLUE;
                    _colourDark = Color.BLUE;
                    break;
                case RegionType.Grassland:
                case RegionType.Forest:
                    _colourLight = Color.GREEN;
                    _colourDark = Color.LIME;
                    break;
            }
        }
        
        public ReturnActions Start() {
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Update() {
            _camera.offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f);

            if (Raylib.GetMouseWheelMove() > 0) {
                _camera.zoom += 0.2f;
            }
            else if (Raylib.GetMouseWheelMove() < 0) {
                _camera.zoom -= 0.2f;
            }
            if (_camera.zoom <= 0.05f) _camera.zoom = 0.05f;
            else if (_camera.zoom > 10f) _camera.zoom = 10f;

            if (Raylib.IsKeyDown(KeyboardKey.KEY_UP)) {
                _camera.target.Y -= 11f - _camera.zoom;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN)) {
                _camera.target.Y += 11f - _camera.zoom;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT)) {
                _camera.target.X -= 11f - _camera.zoom;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT)) {
                _camera.target.X += 11f - _camera.zoom;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE)) {
                Game.ChangeScene(_mapViewer);
            }
            
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Render() {
            Raylib.BeginMode2D(_camera);
            for (int x = 0, i = 0; x < _width; x++, i++) {
                for (int y = 0; y < _height; y++, i++) {
                    Raylib.DrawRectangle(x * 32, y * 32, 32, 32, i % 2 == 0 ? _colourLight : _colourDark);
                }
            }
            Raylib.EndMode2D();
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Close() {
            return ReturnActions.ReturnNull;
        }
    }
}