using UnityEngine;


namespace FPS_Kotikov_D
{
    /// <summary>
    /// Class for rise and move objects (objects whith IInteraction)
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SpringJoint))]
    public class InteractionPoint : BaseObjectScene
    {


        #region Fileds

        const float LERPSMOUTH = 15f;

        [SerializeField, Range(1, 1000)]
        public int SpringJointForceMyltipler = 15;
        [SerializeField, Range(1, 50)]
        public int ThrowForceMultipler = 15;
        [HideInInspector]
        public SpringJoint MySpringJoint;

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
            MySpringJoint = gameObject.GetComponent<SpringJoint>();
            if (MySpringJoint == null)
                MySpringJoint = gameObject.AddComponent<SpringJoint>();
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
            _objRb.AddForce(_objTr.forward * _objRb.mass * ThrowForceMultipler, ForceMode.Impulse);
        }

        public void ReleaseObject()
        {
            if (!_isCatched) return;
            _isCatched = false;

            MySpringJoint.connectedBody = default;
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