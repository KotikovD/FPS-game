namespace FPS_Kotikov_D
{
    public interface IInteraction
    {
        int ThrowForceMultipler { get; set; }
        bool IsRaised { get; set; }

        void Interaction<T>(T value = default) where T : class;


    }
}
