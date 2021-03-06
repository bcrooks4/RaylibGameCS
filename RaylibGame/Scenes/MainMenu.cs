﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Raylib_cs;
using RaylibGame.Engine;

namespace RaylibGame.Scenes {
    internal readonly struct MenuButton {
        public readonly string Text;
        public readonly int ButtonAction;

        public MenuButton(string text, int buttonAction) {
            Text = text;
            ButtonAction = buttonAction;
        }
    }
    
    public class MainMenu : IScene {
        private readonly MenuButton[] _menuButtons = new[] {
            new MenuButton("Play", 0),
            new MenuButton("Options", 1),
            new MenuButton("Credits", 2), 
            new MenuButton("Quit", 3), 
        };

        private const int MenuFontSize = 40;
        private int _highlightedButton;

        private int _frame;

        private Texture2D _backgroundTexture = Raylib.LoadTexture("Resources/background.png");

        public ReturnActions Start() {
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Update() {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP)) {
                _highlightedButton--;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN)) {
                _highlightedButton++;
            }

            if (_highlightedButton < 0) {
                _highlightedButton = _menuButtons.Length - 1;
            }
            else if (_highlightedButton >= _menuButtons.Length) {
                _highlightedButton = 0;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER)) {
                switch (_menuButtons[_highlightedButton].ButtonAction) {
                    case 0:
                        Game.ChangeScene(new MapDrawing());
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        return ReturnActions.ReturnQuit;
                }
            }

            _frame++;
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Render() {
            //Raylib.DrawRectangleGradientV(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), Color.BLUE, Color.DARKBLUE);
            //Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), Raylib.Fade(Color.BLACK, (float)Math.Sin(_frame / 500f) / 3f));

            Raylib.ClearBackground(Color.GOLD);
            
            int size = Math.Max(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            
            Raylib.DrawTextureQuad(_backgroundTexture, 
                new Vector2((Raylib.GetScreenWidth() - 20) / (float)_backgroundTexture.width / 2.5f,
                    (Raylib.GetScreenHeight() - 20) / (float)_backgroundTexture.height / 2.5f), 
                new Vector2((float)Raylib.GetTime() * 0.1f, 0), 
                new Rectangle(10, 10, Raylib.GetScreenWidth() - 20, Raylib.GetScreenHeight() - 20), 
                Color.WHITE);
            
            for (int i = 0; i < _menuButtons.Length; i++) {
                Color colour;
                if (i == _highlightedButton) {
                    colour = Color.WHITE;
                }
                else {
                    colour = new Color(50, 50, 50, 125);
                }

                if (_highlightedButton == i) {
                    Raylib.DrawRectangleGradientH(10,
                        Raylib.GetScreenHeight() - ((_menuButtons.Length - i) * MenuFontSize + 10),
                        300,
                        MenuFontSize,
                        Raylib.Fade(Color.WHITE, 0.5f),
                        Raylib.Fade(Color.WHITE, 0.25f));
                }

                Raylib.DrawText(_menuButtons[i].Text, 
                    20, 
                    Raylib.GetScreenHeight() - ((_menuButtons.Length - i) * MenuFontSize + 10), 
                    MenuFontSize, 
                    colour);
            }
            
            Raylib.DrawRectangleLines(10, 
                10, 
                Raylib.GetScreenWidth() - 20,
                Raylib.GetScreenHeight() - 20,
                Color.MAROON);
            Raylib.DrawRectangleLines(0, 
                0, 
                Raylib.GetScreenWidth(),
                Raylib.GetScreenHeight(),
                Color.YELLOW);
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Close() {
            return ReturnActions.ReturnNull;
        }
    }
}