using UnityEngine;
using DG.Tweening;


namespace FPS_Kotikov_D
{
    [RequireComponent(typeof(BoxCollider))]
    public class DoorOpenAnimation : BaseObjectScene
    {


        #region Fields

        private const float FIXOFFSET = 2f;

        [SerializeField] private float _duration = 5f;
        private float _moveOffset;

        #endregion


        #region Methods

        private void Awake()
        {
            _moveOffset = GetComponent<BoxCollider>().bounds.size.y + FIXOFFSET;
        }

        public void Open()
        {
           transform.DOLocalMoveY(_moveOffset, _duration);
        }

        #endregion


    }
}

