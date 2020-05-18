namespace FPS_Kotikov_D
{
    /// <summary>
    /// Use for pickup things and throw it
    /// </summary>
    public interface IInteraction
    {
        bool IsRaised { get; set; }

        void Interaction<T>(T value = default) where T : class;


    }
}
