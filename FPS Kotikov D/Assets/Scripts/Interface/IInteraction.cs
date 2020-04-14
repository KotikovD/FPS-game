namespace FPS_Kotikov_D
{
    public interface IInteraction
    {
        bool IsRaised { get; set; }

        void Interaction<T>(T value = default) where T : class;


    }
}
