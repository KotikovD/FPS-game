using UnityEngine;
using DG.Tweening;


namespace FPS_Kotikov_D
{
    public class SphereAimation : MonoBehaviour
    {


        #region Fields

        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _moveOffset = 0.5f;

        private Sequence _sequence;

        #endregion


        #region UnityMethods

        private void Start()
        {
            _sequence = DOTween.Sequence();
            _sequence.SetRelative();
            _sequence.Append(transform.DOMoveY(_moveOffset, _duration));
            _sequence.Join(transform.DORotate(new Vector3(0, 90, 90), _duration));
            _sequence.SetLoops(-1, LoopType.Yoyo);
        }

        #endregion


    }
}

