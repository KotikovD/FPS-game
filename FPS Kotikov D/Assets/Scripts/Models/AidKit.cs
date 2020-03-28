using UnityEngine;


namespace FPS_Kotikov_D
{

    public class AidKit : BaseObjectScene, ISerializable
    {

        [SerializeField] private float _healValue = 25.0f;


        private void OnCollisionEnter(Collision collision)
        {
            var tempObj = collision.gameObject.GetComponent<IHeal>();

            if (tempObj != null)
            {
                if (tempObj.Heal(0))
                {
                    tempObj.Heal(_healValue);
                    DestroyAidKit();
                }
            }
        }


        #region Methods

        private void DestroyAidKit()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}