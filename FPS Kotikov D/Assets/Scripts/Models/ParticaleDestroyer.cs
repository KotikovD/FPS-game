using UnityEngine;


namespace FPS_Kotikov_D
{
    public class ParticaleDestroyer : MonoBehaviour
    {

        //private void OnCollisionEnter(Collision collision)
        //{
        //    Destroy(gameObject);
        //}
        private void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }


    }
}