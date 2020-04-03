using UnityEngine;


namespace FPS_Kotikov_D
{
    [RequireComponent(typeof(Rigidbody))]
    public class AidKit : BaseObjectScene, ISerializable, IInteraction, IShowMessage
    {

        [SerializeField] private float _healValue = 25.0f;



        private void OnCollisionEnter(Collision collision)
        {
            var tempObj = collision.gameObject.GetComponent<IHeal>();

            if (tempObj == null) return;
                if (tempObj.Heal(0))
                {
                    tempObj.Heal(_healValue);
                    DestroyAidKit();
                }
            
        }

        #region Methods

        public void Interaction<T>(T value = null) where T : class
        {
            RaiseUp();
        }

        private void DestroyAidKit()
        {
            Destroy(gameObject);
        }

        public void ShowMessage()
        {
            ShowName();
        }

        #endregion
    }
}