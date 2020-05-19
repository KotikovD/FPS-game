using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;


namespace FPS_Kotikov_D
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class ValveEvent : BaseObjectScene, IShowMessage, IInteraction
    {


        #region Fields

        [SerializeField, Tooltip("Some particale object will be scaled to 1,1,1")]
        private GameObject[] _dependenceObjects;
        [SerializeField] private Vector3 _finishRotationValues;
        [SerializeField] private float _duration = 10.0f;
        [SerializeField, Tooltip("If player exceed this value - valve will paused")]
        private float _intaractionDistance = 1.5f;
        [SerializeField] private string _nameForMessage;

        private bool _isInteraction = false;
        private Sequence _sequence;

        #endregion


        #region Methods

        private void Awake()
        {
            base.Awake();
            var rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        private void FixedUpdate()
        {
            if (!_isInteraction) return;
            if (Vector3.Distance(transform.position, PlayerPresenter.Position) < _intaractionDistance)
            {
                if (_sequence == null)
                    StartAnimation();
                else
                    _sequence.Play();
            }
            else if (Vector3.Distance(transform.position, PlayerPresenter.Position) > _intaractionDistance)
            {
                _isInteraction = false;
                if (_sequence != null)
                    _sequence.Pause();
            }
        }

        public void ShowMessage()
        {
            if (_nameForMessage != string.Empty)
                gameObject.name = _nameForMessage;
            ShowName();
        }

        public void Interaction<T>(T value = null) where T : class
        {
            _isInteraction = true;
        }

        private void StartAnimation()
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DORotate(_finishRotationValues, _duration, RotateMode.FastBeyond360));
            foreach (var obj in _dependenceObjects)
                _sequence.Join(obj.transform.DOScale(new Vector3(1, 1, 1), _duration));
        }

        #endregion


    }
}