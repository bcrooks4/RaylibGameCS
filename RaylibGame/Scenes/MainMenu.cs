using System;
using System.Collections.Generic;
using System.Linq;
using Raylib_cs;
using RaylibGame.Engine;

namespace RaylibGame.Scenes {
    internal struct MenuButton {
        public string Text;
        public int ButtonAction;

        public MenuButton(string text, int buttonAction) {
            Text = text;
            ButtonAction = buttonAction;
        }
    }

    public class MainMenu : IScene {
        private MenuButton[] _menuButtons = new[] {
            new MenuButton("Play", 0),
            new MenuButton("Options", 1),
            new MenuButton("Credits", 2), 
            new MenuButton("Quit", 3), 
        };
        private int _menuFontSize = 64;
        private int _highlightedButton = 0;
        
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

            return ReturnActions.ReturnNull;
        }

        public ReturnActions Render() {
            for (int i = 0; i < _menuButtons.Length; i++) {
                Color colour;
                if (i == _highlightedButton) {
                    colour = Color.RAYWHITE;
                }
                else {
                    colour = new Color(50, 50, 50, 255);
                }

                Raylib.DrawText(_menuButtons[i].Text, 
                    20, 
                    Raylib.GetScreenHeight() - ((_menuButtons.Length - i) * _menuFontSize + 20), 
                    _menuFontSize, 
                    colour);
                
                Raylib.DrawRectangleLines(10, 
                    10, 
                    Raylib.GetScreenWidth() - 20,
                    Raylib.GetScreenHeight() - 20,
                    Color.WHITE);
            }
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Close() {
            return ReturnActions.ReturnNull;
        }
    }
}