using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using Raylib_cs;
using RaylibGame.Engine;
using Color = Raylib_cs.Color;

namespace RaylibGame.Scenes {
    public enum DistanceMethod {
        Euclidean,
        Manhattan,
    }
    
    public class MapDrawing : IScene {
        private int[] _inputMap;
        private int _width;
        private int _height;
        private int _mapScale;

        private int _textureScale;

        private Camera2D _camera;
        
        public ReturnActions Start() {
            _width = 16;
            _height = 16;
            _mapScale = 32;
            _inputMap = new int[_width * _height];
            _textureScale = 16;

            _camera = new Camera2D {zoom = 1};

            Raylib.SetTargetFPS(60);
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Update() {
            #region Camera Movement
            
            _camera.offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f);
            
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

            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON)) {
                int x = (int)(Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _camera) / _mapScale).X;
                int y = (int)(Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _camera) / _mapScale).Y;
                if (x >= 0 && x < _width && y >= 0 && y < _height) {
                    _inputMap[y * _width + x] = 1;
                }
            }
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON)) {
                int x = (int)(Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _camera) / _mapScale).X;
                int y = (int)(Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _camera) / _mapScale).Y;
                if (x >= 0 && x < _width && y >= 0 && y < _height) {
                    _inputMap[y * _width + x] = 0;
                }
            }

            #endregion

            #region Map Controlls
            
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL)) {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ONE)) {
                    _width = 8;
                    _height = 8;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_TWO)) {
                    _width = 16;
                    _height = 16;
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_THREE)) {
                    _width = 32;
                    _height = 32;
                }
                _inputMap = new int[_width * _height];
            }
            else {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ONE)) {
                    for (int y = 0; y < _height; y++) {
                        for (int x = 0; x < _width; x++) {
                            _inputMap[y * _width + x] = 0;
                        }
                    }
                }
                
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_TWO)) {
                    for (int y = 0; y < _height; y++) {
                        for (int x = 0; x < _width; x++) {
                            _inputMap[y * _width + x] = 1;
                        }
                    }
                }
                
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_THREE)) {
                    Random random = new Random();
                    for (int y = 0; y < _height; y++) {
                        for (int x = 0; x < _width; x++) {
                            _inputMap[y * _width + x] = random.Next() % 2;
                        }
                    }
                }
                
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_FOUR)) {
                    Random random = new Random();
                    for (int y = 0; y < _height; y++) {
                        for (int x = 0; x < _width; x++) {
                            _inputMap[y * _width + x] = random.Next() % 2;
                        }
                    }
                    
                    for (int i = 0; i < 3; i++) {
                        bool[] cellMap = new bool[_inputMap.Length];
                        for (int y = 0; y < _height; y++) {
                            for (int x = 0; x < _width; x++) {
                                int neighbourCount = 0;
                                if (x > 0) {
                                    if (_inputMap[y * _width + x - 1] > 0) neighbourCount++;
                                }
                                else {
                                    if (_inputMap[y * _width + _width - 1] > 0) neighbourCount++;
                                }

                                if (x < _width - 1) {
                                    if (_inputMap[y * _width + x + 1] > 0) neighbourCount++;
                                }
                                else {
                                    if (_inputMap[y * _width] > 0) neighbourCount++;
                                }

                                if (y > 0) {
                                    if (_inputMap[(y - 1) * _width + x] > 0) neighbourCount++;
                                }
                                else {
                                    if (_inputMap[(_height - 1) * _width + x] > 0) neighbourCount++;
                                }

                                if (y < _width - 1) {
                                    if (_inputMap[(y + 1) * _width + x] > 0) neighbourCount++;
                                }
                                else {
                                    if (_inputMap[_width + x] > 0) neighbourCount++;
                                }

                                if (x > 0 && y > 0) {
                                    if (_inputMap[(y - 1) * _width + x - 1] > 0) neighbourCount++;
                                }
                                else {
                                    if (_inputMap[(_height - 1) * _width + _width - 1] > 0) neighbourCount++;
                                }

                                if (x > 0 && y < _height - 1) {
                                    if (_inputMap[(y + 1) * _width + x - 1] > 0) neighbourCount++;
                                }
                                else {
                                    if (_inputMap[_width - 1] > 0) neighbourCount++;
                                }

                                if (x < _width - 1 && y > 0) {
                                    if (_inputMap[(y - 1) * _width + x + 1] > 0) neighbourCount++;
                                }
                                else {
                                    if (_inputMap[_height - 1] > 0) neighbourCount++;
                                }

                                if (x < _width - 1 && y < _height - 1) {
                                    if (_inputMap[(y + 1) * _width + x + 1] > 0) neighbourCount++;
                                }
                                else {
                                    if (_inputMap[0] > 0) neighbourCount++;
                                }

                                cellMap[y * _width + x] = _inputMap[y * _width + x] > 0;
                                if (_inputMap[y * _width + x] > 0) {
                                    if (neighbourCount < 2) {
                                        cellMap[y * _width + x] = false;
                                    }
                                    else if (neighbourCount > 3) {
                                        cellMap[y * _width + x] = false;
                                    }
                                }
                                else {
                                    if (neighbourCount == 3) {
                                        cellMap[y * _width + x] = true;
                                    }
                                }
                            }
                        }

                        for (int j = 0; j < _inputMap.Length; j++) {
                            _inputMap[j] = cellMap[j] ? 1 : 0;
                        }
                    }
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_FIVE)) {
                    for (int x = 0; x < _width; x++) {
                        _inputMap[x] = 0;
                        _inputMap[(_height - 1) * _width + x] = 0;
                    }
                    for (int y = 0; y < _height; y++) {
                        _inputMap[y * _height] = 0;
                        _inputMap[y * _height + (_width - 1)] = 0;
                    }
                }
            }
            
            #endregion

            #region Export Map Texture
            
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER)) {
                File.Delete("export.png");
                
                Random random = new Random();
                Bitmap bitmap = new Bitmap(_width * _textureScale, _height * _textureScale);

                Vector2[] cellPositions = new Vector2[_width * _height];
                System.Drawing.Color[] cellColours = new System.Drawing.Color[_width * _height];
                for (int y = 0; y < _height; y++) {
                    for (int x = 0; x < _width; x++) {
                        cellPositions[y * _width + x] = new Vector2(
                            x * _textureScale + random.Next() % _textureScale,
                            y * _textureScale + random.Next() % _textureScale);

                        cellColours[y * _width + x] = _inputMap[y * _width + x] == 1
                            ? System.Drawing.Color.Chartreuse
                            : System.Drawing.Color.CornflowerBlue;
                    }
                }
                
                for (int y = 0; y < _height * _textureScale; y++) {
                    for (int x = 0; x < _width * _textureScale; x++) {
                        float closestDistance = float.MaxValue;
                        int closestIndex = 0;

                        for (int i = 0; i < cellPositions.Length; i++) {
                            float distance = GetDistance(new Vector2(x, y), cellPositions[i], DistanceMethod.Manhattan);
                            if (distance < closestDistance) {
                                closestDistance = distance;
                                closestIndex = i;
                            }
                        }

                        var colour = cellColours[closestIndex];
                        bitmap.SetPixel(x, y, colour);
                    }
                }
                
                bitmap.Save("export.png");
                bitmap.Dispose();
                
                Game.ChangeScene(new MapViewer());
            }
            
            #endregion

            #region Quit

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE)) {
                Game.ChangeScene(new MainMenu());
            }

            #endregion

            return ReturnActions.ReturnNull;
        }

        public ReturnActions Render() {
            Raylib.BeginMode2D(_camera);
            for (int y = 0; y < _height; y++) {
                for (int x = 0; x < _width; x++) {
                    Color colour;
                    switch (_inputMap[y * _width + x]) {
                        case 0:
                            colour = Color.SKYBLUE;
                            break;
                        case 1:
                            colour = Color.GREEN;
                            break;
                        default:
                            colour = Color.MAGENTA;
                            break;
                    }

                    Raylib.DrawRectangle(x * _mapScale, y * _mapScale, _mapScale, _mapScale, colour);
                }
            }
            Raylib.EndMode2D();
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Close() {
            Raylib.SetTargetFPS(int.MaxValue);
            return ReturnActions.ReturnNull;
        }

        private float GetDistance(Vector2 a, Vector2 b, DistanceMethod distanceMethod) {
            switch (distanceMethod) {
                case DistanceMethod.Euclidean:
                    return Vector2.DistanceSquared(a, b);
                case DistanceMethod.Manhattan: 
                    float x = Math.Abs(a.X - b.X);
                    float y = Math.Abs(a.Y - b.Y);
                    float dist = x + y;
                    return dist;
                default:
                    return 0f;
            }
        }
    }
}