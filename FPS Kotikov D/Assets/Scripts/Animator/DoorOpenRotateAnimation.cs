using UnityEngine;
using DG.Tweening;


namespace FPS_Kotikov_D
{
    [RequireComponent(typeof(BoxCollider))]
    public class DoorOpenRotateAnimation : BaseObjectScene
    {


        #region Fields

        private const float FIXOFFSET = 2f;

        [SerializeField] private float _duration = 5f;
        [SerializeField] private Vector3 _openAngle;

        #endregion


        #region Methods

        public void Open()
        {
            transform.DOLocalRotate(_openAngle, _duration);
        }

        #endregion


    }
}

