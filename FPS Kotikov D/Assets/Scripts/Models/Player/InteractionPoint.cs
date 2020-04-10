using UnityEngine;


namespace FPS_Kotikov_D
{
    /// <summary>
    /// Class for rise to hands and move objects (objects whith IInteraction)
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SpringJoint))]
    public class InteractionPoint : BaseObjectScene
    {


        #region Fileds

        const float LERPSMOUTH = 15f;

        //[SerializeField, Range(2, 20)] private int _addForceMultiplier = 15;
        private IInteraction _objIi;
        private Rigidbody _objRb;
        private Transform _objTr;
        private BoxCollider _objColider;
        private bool _isCatched = false;

        #endregion


        #region Properties

        public bool IsCatched
        {
            get { return _isCatched; }
            private set { _isCatched = value; }
        }

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            Player.Interaction = gameObject.GetComponent<SpringJoint>();
            var rB = gameObject.GetComponent<Rigidbody>();
            rB.isKinematic = true;
            rB.useGravity = false;
        }

        private void OnTriggerStay(Collider collider)
        {
            if (_isCatched) return;

            _objIi = collider.GetComponent<IInteraction>();

            if (_objIi != null && _objIi.IsRaised)
            {
                _isCatched = true;
                _objColider = collider.GetComponent<BoxCollider>();
                _objColider.isTrigger = true;
                _objRb = collider.GetComponent<Rigidbody>();
                _objRb.velocity = default;
                _objTr = collider.transform;
                _objTr.SetParent(gameObject.transform);
                Player.Interaction.connectedBody = default;
            }
        }

        private void LateUpdate()
        {
            if (!_isCatched) return;
            if (_objColider == null)
            {
                ReleaseObject();
                return;
            }
                
            var objPos = transform.position - _objColider.center;
            var objRot = _objTr.rotation;

            objPos = Vector3.Lerp(_objTr.position, objPos, LERPSMOUTH * Time.deltaTime);
            _objTr.position = objPos;

            objRot = Quaternion.Lerp(objRot, transform.rotation, LERPSMOUTH * Time.deltaTime);
            _objTr.rotation = objRot;
        }

        #endregion


        #region Methods

        public void ThrowObject()
        {
            if (!_isCatched) return;
            ReleaseObject();
            _objRb.AddForce(_objTr.forward * _objRb.mass * _objIi.ThrowForceMultipler, ForceMode.Impulse);
        }

        public void ReleaseObject()
        {
            if (!_isCatched) return;
            _isCatched = false;

            if (_objTr != null)
            {
                _objTr.SetParent(default);
                _objColider.isTrigger = false;
                _objIi.IsRaised = false;
                _objRb.useGravity = true;
            }
        }

        #endregion


    }
}