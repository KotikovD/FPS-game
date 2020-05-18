using UnityEngine;


namespace FPS_Kotikov_D
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class Thing : BaseObjectScene, ISerializable, IInteraction, IShowMessage
    {

       [SerializeField] private string _nameForMessage;

        #region Methods

        public void Interaction<T>(T value = null) where T : class
        {
            RaiseUp();
        }

        public void ShowMessage()
        {
            if (_nameForMessage != default)
                ShowName(_nameForMessage);
            else
                ShowName();
        }

        #endregion
    }
}

