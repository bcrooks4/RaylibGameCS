using Raylib_cs;
using RaylibGame.Engine;

namespace RaylibGame.Scenes {
    public class MapViewer : IScene {
        private Texture2D _mapTexture;
        
        public ReturnActions Start() {
            _mapTexture = Raylib.LoadTexture("export.png");
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Update() {
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Render() {
            Raylib.DrawTexture(_mapTexture, 0, 0, Color.WHITE);
            return ReturnActions.ReturnNull;
        }

        public ReturnActions Close() {
            Raylib.UnloadTexture(_mapTexture);
            return ReturnActions.ReturnNull;
        }
    }
}