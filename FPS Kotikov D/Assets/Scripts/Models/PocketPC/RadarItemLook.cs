using UnityEngine;

namespace FPS_Kotikov_D
{
    public class RadarItemLook : BaseObjectScene
    {

        public Vector3 _viewVector = new Vector3(0, 1000, 0);

        private void LateUpdate()
        {
            transform.LookAt(_viewVector) ;
        }


    }
}
