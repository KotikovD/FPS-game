using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityStandardAssets.Characters.ThirdPerson;


namespace FPS_Kotikov_D
{
    /// <summary>
    ///  AI (Artificial Idiot) bots class
    /// </summary>
    public sealed class Bot : BaseObjectScene, ISetDamage
    {


        #region Fileds

        private const float ERROR_DISTANCE = 0.1f;

        public Vision Vision;
        public event Action<Bot> OnDieChange;
        public float Hp = 100;

        [SerializeField, Range(0, 1)] private float _patrolSpeed = 0.5f;
        [SerializeField] private float _delayAfterDie = 10.0f;
        private float _inspectionTime;
        [SerializeField] private float _minRandomInspectionTime = 1f;
        [SerializeField] private float _maxRandomInspectionTime = 10.0f;
        [SerializeField] private float _maxPatrolTime = 10f;

        [Header("Fighting options")]
        [SerializeField] private float _minRandomFightingDelay = 0.5f;
        [SerializeField] private float _maxRandomFightingDelay = 3.0f;
        [SerializeField] private float _minStoppingDistance = 3f;
        [SerializeField] private float _maxStoppingDistance = 6f;
        [SerializeField, Range(0, 1)] private float _chaseSpeed = 0.7f;
        [SerializeField] private float _distanceAttack = 5f;
        [SerializeField] private ThirdPersonCharacter _botCharacter;
        private Weapons _weapon;
        private Animator _animator;
        private StateBot _stateBot;
        private Vector3 _point;
        private Transform _camera;

        private Vector3 _moveSpeed;
        private bool _isPunching;
        private bool _isFigthingDelay = false;
        private float _currentDistanceToTarget;
        private Rigidbody[] _ragDollparts;

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
                        CancelInvoke(nameof(SetStateBotToNone));
                        break;
                    case StateBot.Patrol:
                        Color = Color.green;
                        Agent.stoppingDistance = 0;
                        Invoke(nameof(SetStateBotToNone), _maxPatrolTime);
                        break;
                    case StateBot.PointInspection:
                        Color = Color.yellow;
                        _inspectionTime = Random.Range(_minRandomInspectionTime, _maxRandomInspectionTime);
                        CancelInvoke(nameof(SetStateBotToNone));
                        Invoke(nameof(SetStateBotToNone), _inspectionTime);
                        break;
                    case StateBot.PlayerDetected:
                        Color = Color.red;
                        Agent.stoppingDistance = Random.Range(_minStoppingDistance, _maxStoppingDistance);
                        CancelInvoke(nameof(SetStateBotToNone));
                        break;
                    case StateBot.PlayerAttack:
                        Color = Color.magenta;
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

            _camera = Camera.main.transform;

            Agent = GetComponent<NavMeshAgent>();
            Agent.updateRotation = false;

            _ragDollparts = GetComponentsInChildren<Rigidbody>();
            SetRagDoll(false);

            var randWeaponNumber = ServiceLocator.Resolve<Inventory>().Length;
            _weapon = ServiceLocator.Resolve<Inventory>().Weapons[Random.Range(0, randWeaponNumber)];
            var localWeapon = Instantiate(_weapon, transform.position, transform.rotation);
            PlaceAndSetWeapons(transform, StringKeeper.WeaponPlace, localWeapon.transform);
            _weapon = localWeapon;
            _weapon.Switch(true);

            _animator = transform.GetComponentInChildren<Animator>();

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

        //private void OnEnable()
        //{
        //    var bodyBot = GetComponentInChildren<BodyBot>();
        //    if (bodyBot != null) bodyBot.OnApplyDamageChange += SetDamage;

        //    var headBot = GetComponentInChildren<HeadBot>();
        //    if (headBot != null) headBot.OnApplyDamageChange += SetDamage;
        //}

        //private void OnDisable()
        //{
        //    var bodyBot = GetComponentInChildren<BodyBot>();
        //    if (bodyBot != null) bodyBot.OnApplyDamageChange -= SetDamage;

        //    var headBot = GetComponentInChildren<HeadBot>();
        //    if (headBot != null) headBot.OnApplyDamageChange -= SetDamage;
        //}

        private void OnAnimatorIK()
        {
            if (!Agent.enabled) return;

            if (StateBot == StateBot.PlayerDetected || StateBot == StateBot.PlayerAttack)
            {
                _animator.SetLookAtWeight(0.5f);
                _animator.SetLookAtPosition(_camera.position);
            }
            else
            {
                _animator.SetLookAtWeight(0f);
            }

            if (StateBot == StateBot.PlayerAttack)
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f) ;
                _animator.SetIKPosition(AvatarIKGoal.RightHand, _camera.position);
            }
            else
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
            }

        }

        #endregion


        #region Lifecycle Methods

        public void Tick()
        {
            if (StateBot == StateBot.Died) return;

            if (StateBot != StateBot.PlayerDetected && StateBot != StateBot.PlayerAttack)
            {
                if (Vision.CanBotSeePlayer(transform, Target))
                {
                    Agent.SetDestination(Target.position);
                    StateBot = StateBot.PlayerDetected;
                }
            }

            if (StateBot == StateBot.PlayerAttack)
            {
                Move(0f);

                if (!_weapon.IsReloading && !_isFigthingDelay)
                {
                    _weapon.Fire(_camera.position);
                    _isFigthingDelay = true;
                    var figthingDelay = Random.Range(_minRandomFightingDelay, _maxRandomFightingDelay);
                    Invoke(nameof(SetFigthingDelay), figthingDelay);
                }

                if (!Vision.CanAttack(transform, Target, Agent.stoppingDistance + ERROR_DISTANCE))
                    StateBot = StateBot.PlayerDetected;  
            }
            else if (StateBot == StateBot.PlayerDetected)
            {
                Agent.SetDestination(Target.position);
                Move(_chaseSpeed);

                if (Vision.CanAttack(transform, Target, Agent.stoppingDistance + ERROR_DISTANCE))
                    StateBot = StateBot.PlayerAttack;

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

                if (Agent.remainingDistance < Agent.stoppingDistance + ERROR_DISTANCE)
                    StateBot = StateBot.PointInspection;
            }
            else if (StateBot == StateBot.PointInspection)
            {
                Move(0f);
            }
        }

        #endregion


        #region Methods

        private void Move(float speed)
        {
            _botCharacter.Move(Agent.velocity.normalized * speed, false, false);
        }

        private void SetFigthingDelay()
        {
            _isFigthingDelay = false;
        }

        private void SetStateBotToNone()
        {
            StateBot = StateBot.None;
        }

        public void SetDamage(InfoCollision info)
        {
            StateBot = StateBot.PlayerDetected;

            if (Hp > 0)
            {
                Hp -= info.Damage;
                return;
            }

            if (Hp <= 0)
            {
                KillBot();
            }
        }

        private void SetRagDoll(bool active)
        {
            foreach (var body in _ragDollparts)
            {
                body.isKinematic = !active;
                body.useGravity = active;
            }
        }

        private void KillBot()
        {
            Move(0);
            StateBot = StateBot.Died;
            Agent.enabled = false;
            _botCharacter.enabled = false;
            _animator.enabled = false;
            SetRagDoll(true);
            
            //_animator.SetBool("Dead", true);
            OnDieChange?.Invoke(this);
            _weapon.WeaponDrop();
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
                }
                else
                    PlaceAndSetWeapons(obj, parent, child);
            }
        }

        #endregion


    }
}