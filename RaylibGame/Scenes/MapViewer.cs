using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using RaylibGame.Engine;

namespace RaylibGame.Scenes {
    public class MapViewer : IScene {
        public List<List<Vector2>> Regions;
        
        private Texture2D _mapTexture;
        private Camera2D _camera;

        private int _highlightedRegion = -1;

        public ReturnActions Start() {
            _mapTexture = Raylib.LoadTexture("export.png");
            _camera = new Camera2D {zoom = 1};
            Raylib.SetTargetFPS(60);
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Update() {
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

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE)) {
                Game.ChangeScene(new MapDrawing());
            }
            
            Vector2 mouseCoordinate = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _camera);
            mouseCoordinate.X = (float)Math.Floor(mouseCoordinate.X);
            mouseCoordinate.Y = (float)Math.Floor(mouseCoordinate.Y);
            _highlightedRegion = -1;
            for (int i = 0; i < Regions.Count; i++) {
                if (Regions[i].Contains(new Vector2(mouseCoordinate.X, mouseCoordinate.Y))) {
                    _highlightedRegion = i;
                }
            }
            
            
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Render() {
            Raylib.BeginMode2D(_camera);
            Raylib.DrawTexture(_mapTexture, 0, 0, Color.WHITE);

            if (_highlightedRegion >= 0) {
                foreach (var coordinate in Regions[_highlightedRegion]) {
                    //Raylib.DrawPixelV(coordinate, Color.WHITE); // No fucking clue why this looks so cool,
                                                            // not what I intended but still cool
                    Raylib.DrawRectangle((int)coordinate.X, 
                        (int)coordinate.Y, 
                        1, 
                        1, 
                        Raylib.Fade(Color.YELLOW, 0.15f));
                }
            }
            Raylib.EndMode2D();

            if (_highlightedRegion >= 0) {
                Raylib.DrawText($"Highlighting region: {_highlightedRegion}", 0, 0, 32, Color.WHITE);
            }

            return ReturnActions.ReturnNull;
        }

        public ReturnActions Close() {
            Raylib.UnloadTexture(_mapTexture);
            return ReturnActions.ReturnNull;
        }
    }
}