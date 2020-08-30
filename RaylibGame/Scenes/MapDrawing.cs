﻿using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Raylib_cs;
using RaylibGame.Engine;
using Color = Raylib_cs.Color;

namespace RaylibGame.Scenes {
    public class MapDrawing : IScene {
        private int[] _inputMap;
        private int _width;
        private int _height;
        private int _mapScale;

        private int _textureScale;

        private Camera2D _camera;
        
        public ReturnActions Start() {
            _width = 8;
            _height = 8;
            _mapScale = 32;
            _inputMap = new int[_width * _height];
            _textureScale = 16;

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
                    Random random = new Random();
                    for (int y = 0; y < _height; y++) {
                        for (int x = 0; x < _width; x++) {
                            _inputMap[y * _width + x] = 0;
                        }
                    }
                }
                
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_TWO)) {
                    Random random = new Random();
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

                                cellMap[y * _width + x] = _inputMap[y * _width + x] > 0 ? true : false;
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

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER)) {
                Bitmap bitmap = new Bitmap(_width * _textureScale, _height * _textureScale);

                for (int y = 0; y < _height * _textureScale; y++) {
                    for (int x = 0; x < _width * _textureScale; x++) {
                        bitmap.SetPixel(x, y, 
                            _inputMap[y / _textureScale * _width + x / _textureScale] == 1 ? 
                            System.Drawing.Color.LawnGreen :  
                            System.Drawing.Color.CornflowerBlue);
                    }
                }
                
                bitmap.Save("export.png");
                bitmap.Dispose();
            }

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
            return ReturnActions.ReturnNull;
        }
    }
}