using FPS_Kotikov_D.Controller;
using UnityEngine;


namespace FPS_Kotikov_D
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class AidKit : PickUps, ISerializable, IInteraction, ICanCollect, IShowMessage
    {


        #region Fields

        [SerializeField] private float _healValue = 25.0f;
        [SerializeField] private string _nameForMessage;
        private Player player;

        #endregion


        #region Properties

        public bool IsCanCollect
        {
            get;
            set;
        }

        #endregion


        #region Methods

        public void GetCollect()
        {
            if (IsCanCollect)
                player = ServiceLocator.Resolve<PlayerController>().Player;

            if (player.Heal())
            {
                player.Heal(_healValue);
                DestroyAidKit();
            }
        }

        private void DestroyAidKit()
        {
            Destroy(gameObject);
        }

        public void Interaction<T>(T value = null) where T : class
        {
            IsCanCollect = _healValue > 0 ? true : false;
            RaiseUp();
            DeactiveIcon();
        }

        public void ShowMessage()
        {
            if (_nameForMessage != string.Empty)
                gameObject.name = _nameForMessage;

            if (IsCanCollect)
                gameObject.name = $"{gameObject.name} {_healValue}";
            ShowName();
            if (_hologramIcon != null && _healValue > 0 && !_isIconActive)
            {
                ActiveIcon();
                StartCoroutine(DeactiveIcon(_iconShowTime));
            }
        }

        #endregion


    }
}
