namespace FPS_Kotikov_D
{
    /// <summary>
    /// Use for object which player can pickup (AidKits, bullets and other) Atention: Use whith IInteraction
    /// </summary>
    public interface ICanCollect
    {
        bool IsCanCollect
        {
            get;
            set;
        }

        void GetCollect();

    }
}