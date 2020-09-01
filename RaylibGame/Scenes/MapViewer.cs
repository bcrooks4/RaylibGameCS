using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Raylib_cs;
using RaylibGame.Engine;
using RaylibGame.Types;
using Color = Raylib_cs.Color;
using Region = RaylibGame.Types.Region;

namespace RaylibGame.Scenes {
    public class MapViewer : IScene {
        public List<Region> Regions;
        private List<Vector2> _trees;
        
        private Texture2D _mapTexture;
        private Texture2D _treeTexture;
        private Camera2D _camera;

        private int _highlightedRegion = -1;

        public ReturnActions Start() {
            _mapTexture = Raylib.LoadTexture("export.png");
            _treeTexture = Raylib.LoadTexture("Resources/tree.png");
            _trees = new List<Vector2>();
            _camera = new Camera2D {zoom = 1};

            Random random = new Random();
            for (int i = 0; i < Regions.Count; i++) {
                if (Regions[i].RegionType == RegionType.Ocean)
                    continue;
                if (random.Next() % 10 == 0) {
                    foreach (var position in Regions[i].RegionLocations) {
                        if (random.Next() % 32 == 0) 
                            _trees.Add(position);
                    }
                }
            }
            
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
                Game.ChangeScene(new MapDrawing());
            }

            Vector2 mouseCoordinate = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _camera);
            mouseCoordinate.X = (float)Math.Floor(mouseCoordinate.X);
            mouseCoordinate.Y = (float)Math.Floor(mouseCoordinate.Y);
            _highlightedRegion = -1;
            for (int i = 0; i < Regions.Count; i++) {
                if (Regions[i].RegionLocations.Contains(new Vector2(mouseCoordinate.X, mouseCoordinate.Y))) {
                    _highlightedRegion = i;
                }
            }
            
            /*
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON)) {
                Random random = new Random();
                foreach (var position in Regions[_highlightedRegion]) {
                    if (random.Next() % 32 == 0) 
                        _trees.Add(position);
                }
            }
            */
            
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Render() {
            Raylib.BeginMode2D(_camera);
            Raylib.DrawTexture(_mapTexture, 0, 0, Color.WHITE);

            foreach (var position in _trees) {
                Color colour;
                colour = _camera.zoom < 8 ? Color.WHITE : Raylib.Fade(Color.WHITE, 0.2f);
                Raylib.DrawTextureV(_treeTexture, position - new Vector2(16, 28), colour);
            }


            if (_highlightedRegion >= 0) {
                foreach (var coordinate in Regions[_highlightedRegion].RegionLocations) {
                    Raylib.DrawPixelV(coordinate, Color.WHITE); // No fucking clue why this looks so cool,
                                                                // not what I intended but still cool
                    Raylib.DrawRectangle((int)coordinate.X, 
                        (int)coordinate.Y, 
                        1, 
                        1, 
                        Raylib.Fade(Color.WHITE, 0.3f));
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
            Raylib.UnloadTexture(_treeTexture);
            return ReturnActions.ReturnNull;
        }
    }
}