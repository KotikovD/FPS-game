using System;
using UnityEngine;
using UnityEngine.AI;

namespace FPS_Kotikov_D
{
    public sealed class Bot : BaseObjectScene
    {

        #region Fileds

        public Vision Vision;
        public Transform WeaponPlace;
        public event Action<Bot> OnDieChange;
        public float Hp = 100;

        [SerializeField] private float _delayAfterDie = 10.0f;

        private Weapons _weapon;
        private StateBot _stateBot;
        private Vector3 _point;
        private float _stoppingDistance = 3.0f;
        private float _waitTime = 3.0f;

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
                        break;
                    case StateBot.Patrol:
                        Color = Color.green;
                        break;
                    case StateBot.Inspection:
                        Color = Color.yellow;
                        break;
                    case StateBot.Detected:
                        Color = Color.red;
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


        #region Methods

        protected override void Awake()
        {
            base.Awake();

            var randWeaponNumber = ServiceLocator.Resolve<Inventory>().Length;
            _weapon = ServiceLocator.Resolve<Inventory>().Weapons[UnityEngine.Random.Range(0, randWeaponNumber)];
            var localWeapon = Instantiate(_weapon, WeaponPlace.position, gameObject.transform.rotation);
            _weapon = localWeapon;
            _weapon.transform.SetParent(WeaponPlace);
            _weapon.Switch(true);

            Agent = GetComponent<NavMeshAgent>();
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

        public void Tick()
        {
            if (StateBot == StateBot.Died) return;

            if (StateBot != StateBot.Detected)
            {
                if (!Agent.hasPath)
                {
                    if (StateBot != StateBot.Inspection)
                    {
                        if (StateBot != StateBot.Patrol)
                        {
                            StateBot = StateBot.Patrol;
                            _point = Patrol.GenericPoint(transform);
                            MovePoint(_point);
                            Agent.stoppingDistance = 0;
                        }
                        else
                        {
                            if (Vector3.Distance(_point, transform.position) <= 1)
                            {
                                StateBot = StateBot.Inspection;
                                Invoke(nameof(ResetStateBot), _waitTime);
                            }
                        }
                    }
                }

                if (Vision.VisionPlayer(transform, Target))
                {
                    StateBot = StateBot.Detected;
                }
            }
            else
            {
                if (Agent.stoppingDistance != _stoppingDistance)
                {
                    Agent.stoppingDistance = _stoppingDistance;
                }

                if (Vision.VisionPlayer(transform, Target))
                {
                    CancelInvoke(nameof(PatrolContinue));
                    _weapon.Fire();
                }
                else
                {
                    
                    MovePoint(Target.position);
                }

                if (Vision.LostPlayer(transform, Target))
                {
                    Invoke(nameof(PatrolContinue), _waitTime);
                }
            }
           
        }

        private void ResetStateBot()
        {
            StateBot = StateBot.None;
        }

        private void PatrolContinue()
        {
            StateBot = StateBot.Patrol;
            MovePoint(_point);
            Agent.stoppingDistance = 0;
        }

        private void SetDamage(InfoCollision info)
        {
            StateBot = StateBot.Detected;

            if (Hp > 0)
            {
                Hp -= info.Damage;
                return;
            }

            if (Hp <= 0)
            {
                StateBot = StateBot.Died;
                Agent.enabled = false;
                foreach (var child in GetComponentsInChildren<Transform>())
                {
                    child.parent = null;

                    var tempRbChild = child.GetComponent<Rigidbody>();
                    if (!tempRbChild)
                    {
                        tempRbChild = child.gameObject.AddComponent<Rigidbody>();
                    }
                    //tempRbChild.AddForce(info.Dir * Random.Range(10, 300));

                    Destroy(child.gameObject, _delayAfterDie);
                }

                OnDieChange?.Invoke(this);
            }
        }

        public void MovePoint(Vector3 point)
        {
            Agent.SetDestination(point);
        }

        #endregion
    }
}