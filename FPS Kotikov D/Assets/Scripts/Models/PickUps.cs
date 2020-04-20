using UnityEngine;
using DG.Tweening;
using System.Collections;


namespace FPS_Kotikov_D
{
    public abstract class PickUps : BaseObjectScene
    {


        #region Fields

        
        protected GameObject _iconInstanse;
        protected bool _isIconActive = false;
        [Header("Set Icon (optional)")]
        [SerializeField] protected GameObject _hologramIcon;
        [SerializeField] protected float _iconShowTime = 5f;
        [SerializeField] private float _iconOffsetY = 1f;
        [SerializeField] private float _iconDuration = 0.3f;

        #endregion


        #region Metodths

        protected void ActiveIcon()
        {
            _isIconActive = true;
            _iconInstanse = Instantiate(_hologramIcon, transform.position, transform.rotation);
            _iconInstanse.transform.SetParent(transform);
            _iconInstanse.transform.DOLocalMoveY(_iconOffsetY, _iconDuration);
        }

        protected IEnumerator DeactiveIcon(float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            _iconInstanse.transform.DOLocalMoveY(-_iconOffsetY, _iconDuration);
            yield return new WaitForSeconds(_iconDuration);
            Destroy(_iconInstanse);
            _isIconActive = false;
            StopCoroutine(nameof(DeactiveIcon));
        }

        #endregion


    }
}