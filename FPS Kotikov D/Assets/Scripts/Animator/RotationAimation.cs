using UnityEngine;
using DG.Tweening;

namespace FPS_Kotikov_D
{

    public class RotationAimation : MonoBehaviour
    {


        #region Fields

        [SerializeField] private float _rotationSpeed = 0.5f;

        #endregion


        #region UnityMethods

        private void Start()
        {
            transform.DORotate(new Vector3 (0, _rotationSpeed, 0), 0, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
        }

        #endregion


    }
}