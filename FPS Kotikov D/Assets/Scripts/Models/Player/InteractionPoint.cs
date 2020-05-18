using FPS_Kotikov_D.Controller;
using UnityEngine;


namespace FPS_Kotikov_D
{
    /// <summary>
    /// Class for rise and move objects (objects whith IInteraction)
    /// </summary>
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
        private ICanCollect _objCanCollect;
        private Rigidbody _objRb;
        private Transform _objTr;
        private BoxCollider _objColider;
        private Vector3 _mainPlace;
        private SphereCollider _myCollider;

        #endregion


        #region Properties

        public bool ObjCanCollect
        {
            get
            {
                if (_objCanCollect != null && _objCanCollect.IsCanCollect)
                    return true;
                else
                    return false;
            }
        }

        public bool IsCatched
        {
            get;
            private set;
        }

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            IsCatched = false;
            MySpringJoint = gameObject.GetComponent<SpringJoint>();
            if (MySpringJoint == null)
                MySpringJoint = gameObject.AddComponent<SpringJoint>();

            _myCollider = GetComponent<SphereCollider>();

            var rB = gameObject.GetComponent<Rigidbody>();
            rB.isKinematic = true;
            rB.useGravity = false;

            _mainPlace = transform.localPosition;
        }

        private void OnTriggerStay(Collider collider)
        {
            if (IsCatched) return;

            _objIi = collider.GetComponent<IInteraction>();

            if (_objIi != null && _objIi.IsRaised)
            {
                IsCatched = true;
                _objColider = collider.GetComponent<BoxCollider>();
                _objColider.isTrigger = true;

                _objCanCollect = collider.GetComponent<ICanCollect>();
                if (ObjCanCollect)
                {
                    var player = transform.GetComponentInParent<CharacterController>();
                    transform.position = player.transform.position;
                }
                else
                {
                    ServiceLocator.Resolve<WeaponController>().Off();
                    ServiceLocator.Resolve<PocketPCController>().Off();
                }
                    _objRb = collider.GetComponent<Rigidbody>();
                    _objRb.velocity = default;
                    _objTr = collider.transform;
                    _objTr.SetParent(gameObject.transform);
                
            }
        }

        private void LateUpdate()
        {
            if (!IsCatched) return;
            if (_objColider == null)
                ReleaseObject();

            var objPos = transform.position - _objColider.center;
            var objRot = _objTr.rotation;

            objPos = Vector3.Lerp(_objTr.position, objPos, LERPSMOUTH * Time.deltaTime);
            _objTr.position = objPos;

            objRot = Quaternion.Lerp(objRot, transform.rotation, LERPSMOUTH * Time.deltaTime);
            _objTr.rotation = objRot;

            if (ObjCanCollect)
                if (_objRb.velocity == default)
                {
                    _objCanCollect.GetCollect();
                    ReleaseObject();
                }
        }

        #endregion


        #region Methods

        public void ThrowObject()
        {
            if (!IsCatched) return;
            ReleaseObject();
            _objRb.AddForce(_objTr.forward * _objRb.mass * ThrowForceMultipler, ForceMode.Impulse);
        }

        public void ReleaseObject()
        {
            if (!IsCatched) return;
            IsCatched = false;

            if (transform.localPosition != _mainPlace)
                transform.localPosition = _mainPlace;

            if (!_myCollider.enabled)
                _myCollider.enabled = true;

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