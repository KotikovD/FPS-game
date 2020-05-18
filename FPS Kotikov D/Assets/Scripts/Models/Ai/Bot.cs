using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityStandardAssets.Characters.ThirdPerson;


namespace FPS_Kotikov_D
{
    public sealed class Bot : BaseObjectScene
    {

        #region Fileds

        private const float PUNCHDISTANCE = 2f;
        private const float ERRORDISTANCE = 0.1f;

        public Vision Vision;
        public event Action<Bot> OnDieChange;
        public float Hp = 100;
        [SerializeField, Range(0, 1)] private float _patrolSpeed = 0.5f;
        [SerializeField] private float _delayAfterDie = 10.0f;
        //[SerializeField] private LayerMask _rayLayerForFoots;
        //[SerializeField] private float _raysLenth = 0.5f;
        //[SerializeField] private float _smoothLerpForFoots = 0.5f;
        private float _inspectionTime;
        [SerializeField] private float _minRandomInspectionTime = 1f;
        [SerializeField] private float _maxRandomInspectionTime = 10.0f;
        [SerializeField] private float _maxPatrolTime = 10f;

        [Header("Fighting options")]
        [SerializeField] private float _minRandomFightingDelay = 0.3f;
        [SerializeField] private float _maxRandomFightingDelay = 2.0f;
        [SerializeField] private float _minStoppingDistance = 1f;
        [SerializeField] private float _maxStoppingDistance = 5f;
        [SerializeField, Range(0, 1)] private float _chaseSpeed = 0.7f;

        [SerializeField] private ThirdPersonCharacter _botCharacter;
        private Weapons _weapon;
        private Animator _animator;
        private StateBot _stateBot;
        private Vector3 _point;

        private Vector3 _moveSpeed;
        private bool _isPunching;
        private bool _isFigthingDelay = false;
        private float _currentDistanceToTarget;

        //private Transform _leftLeg;
        //private Transform _rightLeg;
        //private Quaternion _rightLegRotation;
        //private Vector3 _rightLegPosition;
        //private Quaternion _leftLegRotation;
        //private Vector3 _leftLegPosition;

        #endregion


        #region Properties

        public Transform Target { get; set; }
        public NavMeshAgent Agent { get; private set; }

        private StateBot StateBot
        {
            get => _stateBot;
            set
            {
                _stateBot = value;
                switch (value)
                {
                    case StateBot.None:
                        Color = Color.white;
                        CancelInvoke();
                        break;
                    case StateBot.Patrol:
                        Color = Color.green;
                        Agent.stoppingDistance = 0;
                        Invoke(nameof(SetStateBotToNone), _maxPatrolTime);
                        break;
                    case StateBot.PointInspection:
                        Color = Color.yellow;
                        _inspectionTime = Random.Range(_minRandomInspectionTime, _maxRandomInspectionTime);
                        CancelInvoke();
                        Invoke(nameof(SetStateBotToNone), _inspectionTime);
                        break;
                    case StateBot.PlayerDetected:
                        Color = Color.red;
                        Agent.stoppingDistance = Random.Range(_minStoppingDistance, _maxStoppingDistance);
                        CancelInvoke();
                        break;
                    case StateBot.Died:
                        Color = Color.gray;
                        break;
                    default:
                        Color = Color.white;
                        break;
                }

            }
        }

        #endregion


        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();

            Agent = GetComponent<NavMeshAgent>();
            Agent.updateRotation = false;

            var randWeaponNumber = ServiceLocator.Resolve<Inventory>().Length;
            _weapon = ServiceLocator.Resolve<Inventory>().Weapons[Random.Range(0, randWeaponNumber)];
            var localWeapon = Instantiate(_weapon, transform.position, transform.rotation);
            PlaceAndSetWeapons(transform, "WeaponPlace", localWeapon.transform);
            _weapon = localWeapon;
            _weapon.Switch(true);

            _animator = transform.GetComponentInChildren<Animator>();
            //_rightLeg = _animator.GetBoneTransform(HumanBodyBones.RightFoot);
            //_leftLeg = _animator.GetBoneTransform(HumanBodyBones.LeftFoot);

            //GenericPoint(Agent.transform, out var point);
            //_point = point;

            switch (_weapon.GunType)
            {
                case GunType.Pistol:
                    _animator.SetBool("IsPistol", true);
                    break;

                case GunType.Rifle:
                    _animator.SetBool("IsRifle", true);
                    break;

                default:
                    break;
            }
        }

        private void OnEnable()
        {
            var bodyBot = GetComponentInChildren<BodyBot>();
            if (bodyBot != null) bodyBot.OnApplyDamageChange += SetDamage;

            var headBot = GetComponentInChildren<HeadBot>();
            if (headBot != null) headBot.OnApplyDamageChange += SetDamage;
        }

        private void OnDisable()
        {
            var bodyBot = GetComponentInChildren<BodyBot>();
            if (bodyBot != null) bodyBot.OnApplyDamageChange -= SetDamage;

            var headBot = GetComponentInChildren<HeadBot>();
            if (headBot != null) headBot.OnApplyDamageChange -= SetDamage;
        }

        private void OnAnimatorIK()
        {
            if (!Agent.enabled) return;

            //var weightRightFoot = _animator.GetFloat("RightLegIK");
            //var weightLeftFoot = _animator.GetFloat("LeftLegIK");
            // Debug.Log("weightRightFoot " + weightRightFoot);

            //_animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weightRightFoot);
            //_animator.SetIKPosition(AvatarIKGoal.RightFoot, _rightLegPosition + new Vector3 (0, 0.1f,0));

            //_animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weightLeftFoot);
            //_animator.SetIKPosition(AvatarIKGoal.LeftFoot, _leftLegPosition + new Vector3(0, 0.1f, 0));

            //_animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, weightRightFoot);
            //_animator.SetIKRotation(AvatarIKGoal.RightFoot, _rightLegRotation);

            //_animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, weightLeftFoot);
            //_animator.SetIKRotation(AvatarIKGoal.LeftFoot, _leftLegRotation);


            if (StateBot == StateBot.PlayerDetected)
            {
                _animator.SetLookAtWeight(0.5f);
                _animator.SetLookAtPosition(Target.position);

                if (_currentDistanceToTarget > PUNCHDISTANCE)
                {
                    _weapon.transform.LookAt(Target.position);
                    _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.3f);
                    _animator.SetIKPosition(AvatarIKGoal.RightHand, Target.position);
                    _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                    _animator.SetIKPosition(AvatarIKGoal.LeftHand, _weapon.LeftHandPosition.position);
                }
            }
            else
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
                _animator.SetLookAtWeight(0f);
            }

        }

        #endregion


        #region Methods



        public void Tick()
        {
            if (StateBot == StateBot.Died) return;

            if (StateBot != StateBot.PlayerDetected)
                if (Vision.CanBotSeePlayer(transform, Target))
                {
                    Agent.SetDestination(Target.position);
                    StateBot = StateBot.PlayerDetected;
                }


            if (StateBot == StateBot.PlayerDetected)
            {
                Agent.SetDestination(Target.position);
                Move(_chaseSpeed);

                if (Vision.BotLostPlayer(transform, Target))
                    SetStateBotToNone();
            }
            else if (StateBot == StateBot.None)
            {
                Move(0f);
                if (Patrol.GenericNewPoint(out var point, Agent.transform.position)
                    && Vector3.Distance(transform.position, point) > Agent.stoppingDistance)
                {
                    StateBot = StateBot.Patrol;
                    _point = point;
                    Agent.SetDestination(_point);
                }
            }
            else if (StateBot == StateBot.Patrol)
            {
                Move(_patrolSpeed);
                //
                //
                if (Agent.remainingDistance < Agent.stoppingDistance + ERRORDISTANCE)
                {
                    StateBot = StateBot.PointInspection;
                }
            }
            else if (StateBot == StateBot.PointInspection)
            {
                Move(0f);
            }


        }

        private void Move(float speed)
        {
            _botCharacter.Move(Agent.velocity.normalized * speed, false, false);
        }


        //public void OldTick()
        //{
        //    if (StateBot == StateBot.Died) return;

        //    if (StateBot != StateBot.PlayerDetected)
        //    {
        //        if (Agent.hasPath)
        //        {

        //            if (StateBot != StateBot.PointInspection)
        //            {
        //                if (StateBot != StateBot.Patrol)
        //                {
        //                    if (Patrol.GenericNewPoint(out _point, Agent.transform.position))
        //                    {
        //                        StateBot = StateBot.Patrol;
        //                        Debug.Log("point " + _point);

        //                    }
        //                    else
        //                        StateBot = StateBot.None;
        //                }
        //                else
        //                {
        //                    Agent.stoppingDistance = 0;
        //                    if (Vector3.Distance(_point, transform.position) < 1)
        //                    {
        //                        StateBot = StateBot.PointInspection;
        //                        Invoke(nameof(SetStateBotToNone), _inspectionTime);
        //                    }
        //                }
        //            }
        //        }

        //        if (Vision.CanBotSeePlayer(transform, Target))
        //            StateBot = StateBot.PlayerDetected;
        //    }
        //    else
        //    {

        //        // Add bot rotation to target
        //        Agent.stoppingDistance = Random.Range(_minStoppingDistance, _maxStoppingDistance);

        //        if (Vision.CanBotSeePlayer(transform, Target))
        //        {
        //            CancelInvoke(nameof(PatrolContinue));
        //            _animator.SetBool("IsReload", _weapon.IsReloading);

        //            _currentDistanceToTarget = Vector3.Distance(Target.position, transform.position);
        //            _animator.SetFloat("Distance", _currentDistanceToTarget, 0.1f, Time.deltaTime);

        //            if (!_weapon.IsReloading && !_isFigthingDelay)
        //            {
        //                _animator.SetTrigger("Shooting");
        //                _weapon.Fire(Target.position);
        //                _isFigthingDelay = true;

        //                var figthingDelay = Random.Range(_minRandomFightingDelay, _maxRandomFightingDelay);
        //                Invoke(nameof(SetFigthingDelay), figthingDelay);
        //            }
        //        }
        //        else
        //        {
        //            Agent.SetDestination(Target.position);
        //        }

        //        if (Vision.BotLostPlayer(transform, Target))
        //            Invoke(nameof(PatrolContinue), _inspectionTime);
        //    }

        //    _animator.SetFloat("VelocityX", Agent.velocity.normalized.magnitude, 0.1f, Time.deltaTime);


        //    //if (Time.frameCount % 2 == 0)
        //    //{
        //    //    var posR = _rightLeg.TransformPoint(Vector3.zero);
        //    //    Debug.DrawRay(posR, Vector3.down * _raysLenth, Color.red);
        //    //    if (Physics.Raycast(posR, Vector3.down, out var rightHit, _raysLenth, _rayLayerForFoots))
        //    //    {
        //    //        _rightLegRotation = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
        //    //        _rightLegPosition = Vector3.Lerp(_rightLeg.position, rightHit.point, _smoothLerpForFoots);
        //    //    }
        //    //}

        //    //if (Time.frameCount % 2 != 0)
        //    //{
        //    //    var posL = _leftLeg.TransformPoint(Vector3.zero);
        //    //    Debug.DrawRay(posL, Vector3.down * _raysLenth, Color.red);
        //    //    if (Physics.Raycast(posL, Vector3.down, out var leftHit, _raysLenth, _rayLayerForFoots))
        //    //    {
        //    //        _leftLegRotation = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
        //    //        _leftLegPosition = Vector3.Lerp(_leftLeg.position, leftHit.point, _smoothLerpForFoots);
        //    //    }
        //    //}
        //}

        private void SetFigthingDelay()
        {
            _isFigthingDelay = false;
        }

        private void SetStateBotToNone()
        {
            StateBot = StateBot.None;
        }

        //private void PatrolContinue()
        //{
        //    StateBot = StateBot.Patrol;
        //    Agent.SetDestination(_point);
        //}

        private void SetDamage(InfoCollision info)
        {
            StateBot = StateBot.PlayerDetected;

            if (Hp > 0)
            {
                Hp -= info.Damage;
                return;
            }

            if (Hp <= 0)
            {
                StateBot = StateBot.Died;
                Agent.enabled = false;
                OnDieChange?.Invoke(this);
                _animator.SetInteger("Dead", 1);
            }
        }



        private void PlaceAndSetWeapons(Transform gameObj, string parent, Transform child)
        {
            foreach (Transform obj in gameObj)
            {
                if (obj.name.Equals(parent))
                {
                    child.SetParent(obj);
                    child.position = obj.position;
                    child.rotation = obj.rotation;
                    // child.localScale = obj.localScale;
                }
                else
                    PlaceAndSetWeapons(obj, parent, child);
            }
        }

        #endregion
    }
}