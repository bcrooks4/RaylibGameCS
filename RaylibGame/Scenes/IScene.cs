namespace RaylibGame.Scenes {
    public interface IScene {
        void Start();
        void Update();
        void Render();
        void Close();
    }
}