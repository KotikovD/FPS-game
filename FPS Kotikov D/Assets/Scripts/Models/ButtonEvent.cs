using UnityEngine;
using UnityEngine.Events;


namespace FPS_Kotikov_D
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class ButtonEvent : BaseObjectScene, IShowMessage, IInteraction
    {

        #region Fields

        [SerializeField] private UnityEvent _buttonInteraction;
        [SerializeField] private string _nameForMessage;

        #endregion


        #region Methods

        private void Awake()
        {
            var rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        public void ShowMessage()
        {
            if (_nameForMessage != string.Empty)
                gameObject.name = _nameForMessage;
            ShowName();
        }

        public void Interaction<T>(T value = null) where T : class
        {
            if (_buttonInteraction != null)
                _buttonInteraction.Invoke();
#if UNITY_EDITOR  
            else
                throw new System.Exception("Add methods to ButtonEvent");
#endif
        }

        #endregion


    }
}