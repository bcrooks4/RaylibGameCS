﻿using System;
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
        private int _selectedRegion = -1;

        private bool _showRegionInformation;

        public ReturnActions Start() {
            _mapTexture = Raylib.LoadTexture("export.png");
            _treeTexture = Raylib.LoadTexture("Resources/tree.png");
            _trees = new List<Vector2>();
            _camera = new Camera2D {zoom = 1};

            Random random = new Random();
            for (int i = 0; i < Regions.Count; i++) {
                if (Regions[i].RegionType == RegionType.Forest) {
                    foreach (var position in Regions[i].RegionLocations) {
                        if (random.Next() % 128 == 0) {
                            _trees.Add(position);
                        }
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

            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON)) {
                if (_highlightedRegion >= 0) {
                    Console.WriteLine(Regions[_highlightedRegion].RegionName);
                    _showRegionInformation = true;
                    _selectedRegion = _highlightedRegion;
                }
                else {
                    if (Raylib.GetMouseX() > 0 && Raylib.GetMouseX() < 400 && Raylib.GetMouseY() > 0 && Raylib.GetMouseY() < Raylib.GetScreenHeight()) {
                        Raylib.DrawRectangle(20, Raylib.GetScreenHeight() - 60, 360, 40, Color.WHITE);
                        if (Raylib.GetMouseX() > 20 &&
                            Raylib.GetMouseY() > Raylib.GetScreenHeight() - 60 &&
                            Raylib.GetMouseX() < 380 &&
                            Raylib.GetMouseY() < Raylib.GetScreenHeight() - 20) {
                            RegionViewer regionViewer = new RegionViewer(32, 32, Regions[_selectedRegion].RegionType, this);
                            Game.ChangeScene(regionViewer);
                        }
                    }
                    else {
                        _showRegionInformation = false;
                        _highlightedRegion = -1;
                    }
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
                    //Raylib.DrawPixelV(coordinate, Color.WHITE); // No fucking clue why this looks so cool,
                                                                // not what I intended but still cool
                    Raylib.DrawRectangle((int)coordinate.X, 
                        (int)coordinate.Y, 
                        1, 
                        1, 
                        Raylib.Fade(Color.WHITE, 0.25f));
                }
            }
            if (_selectedRegion >= 0) {
                foreach (var coordinate in Regions[_selectedRegion].RegionLocations) {
                    //Raylib.DrawPixelV(coordinate, Color.WHITE); // No fucking clue why this looks so cool,
                    // not what I intended but still cool
                    Raylib.DrawRectangle((int)coordinate.X, 
                        (int)coordinate.Y, 
                        1, 
                        1, 
                        Raylib.Fade(Color.WHITE, 0.35f));
                }
            }
            Raylib.EndMode2D();

            if (_showRegionInformation && _selectedRegion >= 0) {
                Raylib.DrawRectangle(0, 0, 400, Raylib.GetScreenHeight(), Color.PURPLE);
                string text = Regions[_selectedRegion].RegionName;
                int x = 200 - Raylib.MeasureText(text, 32) / 2;
                Raylib.DrawText(text, x, 10, 32, Color.WHITE);
                Raylib.DrawLineEx(new Vector2(x, 44), new Vector2(x + Raylib.MeasureText(text, 32), 44), 4f, Color.WHITE);
                
                Raylib.DrawRectangle(20, Raylib.GetScreenHeight() - 60, 360, 40, Color.WHITE);
                text = "View Region";
                x = 200 - Raylib.MeasureText(text, 32) / 2;
                Raylib.DrawText(text, x, Raylib.GetScreenHeight() - 55, 32, Color.BLACK);
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