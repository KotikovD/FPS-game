using UnityEngine;


namespace FPS_Kotikov_D
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class Thing : BaseObjectScene, ISerializable, IInteraction
    {
        

        #region Methods

        public void Interaction<T>(T value = null) where T : class
        {
            RaiseUp();
        }

        #endregion
    }
}

