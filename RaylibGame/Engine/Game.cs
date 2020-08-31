using System;
using System.Numerics;
using Raylib_cs;
using RaylibGame.Scenes;

namespace RaylibGame.Engine {
    public class Game {
        private static int _windowedWidth = 0;
        private static int _windowedHeight = 0;
        private static bool _fullscreen = false;

        private static IScene _currentScene = null;
        private static IScene _nextScene = null;
        
        public void Start() {
            Raylib.SetConfigFlags(ConfigFlag.FLAG_WINDOW_RESIZABLE);
            Raylib.InitWindow(800, 600, "GAME [FPS:000]");
            //Raylib.SetTargetFPS(60);
            Raylib.SetWindowMinSize(400, 200);
            Raylib.SetExitKey(KeyboardKey.KEY_GRAVE);
            
            ChangeScene(new SplashScreen());
        }

        public bool Update() {
            string format = "0000.##";
            Raylib.SetWindowTitle(string.Format("GAME [FPS:{0}]", Raylib.GetFPS().ToString(format)));

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_F10)) {
                ToggleFullscreen();
            }
            
            if (_nextScene != null) {
                _currentScene?.Close();
                
                _currentScene = _nextScene;
                _nextScene = null;

                switch (_currentScene?.Start()) {
                    case ReturnActions.ReturnError:
                        break;
                    case ReturnActions.ReturnQuit:
                        ChangeScene(null);
                        return true;
                    case ReturnActions.ReturnNull:
                        break;
                    case null:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            switch (_currentScene?.Update()) {
                case ReturnActions.ReturnError:
                    break;
                case ReturnActions.ReturnQuit:
                    ChangeScene(null);
                    return true;
                case ReturnActions.ReturnNull:
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }

        public bool Render() {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);
            if (_nextScene == null) {
                switch (_currentScene?.Render()) {
                    case ReturnActions.ReturnError:
                        break;
                    case ReturnActions.ReturnQuit:
                        ChangeScene(null);
                        return true;
                    case ReturnActions.ReturnNull:
                        break;
                    case null:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Raylib.EndDrawing();
            return false;
        }

        public void Close() {
            _currentScene?.Close();
            Raylib.CloseWindow();
        }

        public static void ToggleFullscreen() {
            if (_fullscreen == false) {
                _fullscreen = true;
                _windowedWidth = Raylib.GetScreenWidth();
                _windowedHeight = Raylib.GetScreenHeight();
                
                Raylib.CloseWindow();
                Raylib.SetConfigFlags(ConfigFlag.FLAG_WINDOW_UNDECORATED |
                                      ConfigFlag.FLAG_WINDOW_ALWAYS_RUN);
                
                Raylib.InitWindow(_windowedWidth, _windowedHeight, "");
                Raylib.SetWindowSize(Raylib.GetMonitorWidth(0), Raylib.GetMonitorHeight(0));
                Raylib.SetWindowPosition(0, 0);
            }
            else {
                _fullscreen = false;
                
                Raylib.CloseWindow();
                Raylib.SetConfigFlags(ConfigFlag.FLAG_WINDOW_RESIZABLE |
                                      ConfigFlag.FLAG_WINDOW_ALWAYS_RUN);
                
                Raylib.InitWindow(_windowedWidth, _windowedHeight, "");
            }
            //Raylib.SetTargetFPS(60);
            Raylib.SetExitKey(KeyboardKey.KEY_GRAVE);
        }

        public static void ChangeScene(IScene scene) {
            _nextScene = scene;
        }
    }
}