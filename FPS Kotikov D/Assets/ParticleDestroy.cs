using UnityEngine;


namespace FPS_Kotikov_D
{
    public class ParticleDestroy : MonoBehaviour
    {

        #region Metothds

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<Player>())
                Destroy(gameObject);
        }

        #endregion

    }
}