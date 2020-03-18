using UnityEngine;
using FPS_Kotikov_D.Controller;


namespace FPS_Kotikov_D
{

    public class TestBall : MonoBehaviour
    {

      //  Vector3 point;

        #region UnityMethods

        private void Start()
        {
           // point = ServiceLocator.Resolve<PlayerController>().HitPoint;
        }


        private void LateUpdate()
        {
            gameObject.transform.position = ServiceLocator.Resolve<PlayerController>().HitPoint;
        }

        #endregion



    }
}