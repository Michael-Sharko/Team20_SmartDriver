namespace Shark.Gameplay.Player
{
    public interface IInput
    {
        public float HInput { get; }
        public float VInput { get; }
        public bool SpaceInput { get; }
    }
}