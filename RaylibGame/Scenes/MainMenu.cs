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
            new MenuButton("Options", 0),
            new MenuButton("Credits", 0), 
            new MenuButton("Quit", 0), 
        };
        private int _menuFontSize = 64;
        
        public void Start() {
        }

        public void Update() {
        }

        public void Render() {
            for (int i = 0; i < _menuButtons.Length; i++) {
                Raylib.DrawText(_menuButtons[i].Text, 
                    5, 
                    Raylib.GetScreenHeight() - (_menuButtons.Length - i) * _menuFontSize, 
                    _menuFontSize, 
                    Color.WHITE);
            }
        }

        public void Close() {
        }
    }
}