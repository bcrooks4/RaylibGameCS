using RaylibGame.Engine;

namespace RaylibGame.Scenes {
    public interface IScene {
        ReturnActions Start();
        ReturnActions Update();
        ReturnActions Render();
        ReturnActions Close();
    }
}