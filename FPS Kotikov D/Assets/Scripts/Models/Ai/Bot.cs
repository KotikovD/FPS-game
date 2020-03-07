using System;
using UnityEngine;
using UnityEngine.AI;

namespace FPS_Kotikov_D
{
    public sealed class Bot : BaseObjectScene
    {

        #region Fileds

        public float Hp = 100;
        public Vision Vision;
        public Weapons Weapon; //todo с разным оружием 
        public Transform Target { get; set; }
        public NavMeshAgent Agent { get; private set; }
        public event Action<Bot> OnDieChange;


        private float _waitTime = 3;
        private StateBot _stateBot;
        private Vector3 _point;
        private float _stoppingDistance = 2.0f;

        #endregion


        #region Properties

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

                if (Vision.VisionM(transform, Target))
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
                if (Vision.VisionM(transform, Target))
                {
                    Weapon.Fire();
                }
                else
                {
                    MovePoint(Target.position);
                }

                //todo Потеря персонажа
            }
        }

        private void ResetStateBot()
        {
            StateBot = StateBot.None;
        }

        private void SetDamage(InfoCollision info)
        {
            //todo реакциия на попадание  
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
                    
                    Destroy(child.gameObject, 10);
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