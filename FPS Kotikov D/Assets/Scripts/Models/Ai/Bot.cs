using System;
using UnityEngine;
using UnityEngine.AI;

namespace FPS_Kotikov_D
{
    public sealed class Bot : BaseObjectScene
    {

        #region Fileds

        private const float PUNCHDISTANCE = 2f;

        public Vision Vision;
        public event Action<Bot> OnDieChange;
        public float Hp = 100;

        [SerializeField] private float _delayAfterDie = 10.0f;
        [SerializeField] private LayerMask _rayLayerForFoots;
        [SerializeField] private float _raysLenth = 0.5f;
        [SerializeField] private float _smoothLerpForFoots = 0.5f;

        private Weapons _weapon;
        private Animator _animator;
        private StateBot _stateBot;
        private Vector3 _point;
        private float _stoppingDistance = 3.0f;
        private float _waitTime = 3.0f;
        private Vector3 _move;
        private bool _isPunching;
        private float _currentDistanceToTarget;
        private Transform _leftLeg;
        private Transform _rightLeg;
        private Quaternion _rightLegRotation;
        private Vector3 _rightLegPosition;
        private Quaternion _leftLegRotation;
        private Vector3 _leftLegPosition;

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
                        Agent.speed = 2f;
                        break;
                    case StateBot.Inspection:
                        Color = Color.yellow;
                        Agent.speed = 2f;
                        break;
                    case StateBot.Detected:
                        Color = Color.red;
                        Agent.speed = 3.5f;
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

            var randWeaponNumber = ServiceLocator.Resolve<Inventory>().Length;
            _weapon = ServiceLocator.Resolve<Inventory>().Weapons[UnityEngine.Random.Range(0, randWeaponNumber)];
            var localWeapon = Instantiate(_weapon, transform.position, transform.rotation);
            PlaceAndSetWeapons(transform, "WeaponPlace", localWeapon.transform);
            _weapon = localWeapon;
            _weapon.Switch(true);

            _animator = transform.GetComponentInChildren<Animator>();
            _rightLeg = _animator.GetBoneTransform(HumanBodyBones.RightFoot);
            _leftLeg = _animator.GetBoneTransform(HumanBodyBones.LeftFoot);

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
                    Agent.stoppingDistance = _stoppingDistance;


                // Add bot rotation to target

                if (Vision.VisionPlayer(transform, Target))
                {
                    CancelInvoke(nameof(PatrolContinue));


                    // transform.LookAt(Target, Vector3.up);
                    _animator.SetBool("IsReload", _weapon.IsReloading);

                    _currentDistanceToTarget = Vector3.Distance(Target.position, transform.position);
                    _animator.SetFloat("Distance", _currentDistanceToTarget, 0.1f, Time.deltaTime);

                    if (!_weapon.IsReloading)
                    {
                        _animator.SetTrigger("Shooting");
                        _weapon.Fire();
                    }

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

            _animator.SetFloat("VelocityX", Agent.velocity.normalized.magnitude, 0.1f, Time.deltaTime);


            if (Time.frameCount % 2 == 0)
            {
                var posR = _rightLeg.TransformPoint(Vector3.zero);
                Debug.DrawRay(posR, Vector3.down * _raysLenth, Color.red);
                if (Physics.Raycast(posR, Vector3.down, out var rightHit, _raysLenth, _rayLayerForFoots))
                {
                    _rightLegRotation = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
                    _rightLegPosition = Vector3.Lerp(_rightLeg.position, rightHit.point, _smoothLerpForFoots);
                }
            }

            if (Time.frameCount % 2 != 0)
            {
                var posL = _leftLeg.TransformPoint(Vector3.zero);
                Debug.DrawRay(posL, Vector3.down * _raysLenth, Color.red);
                if (Physics.Raycast(posL, Vector3.down, out var leftHit, _raysLenth, _rayLayerForFoots))
                {
                    _leftLegRotation = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
                    _leftLegPosition = Vector3.Lerp(_leftLeg.position, leftHit.point, _smoothLerpForFoots);
                }
            }



        }

        private void OnAnimatorIK()
        {
            if (!Agent.enabled) return;

            var weightRightFoot = _animator.GetFloat("RightLegIK");
            var weightLeftFoot = _animator.GetFloat("LeftLegIK");
           // Debug.Log("weightRightFoot " + weightRightFoot);

            //_animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weightRightFoot);
            //_animator.SetIKPosition(AvatarIKGoal.RightFoot, _rightLegPosition + new Vector3 (0, 0.1f,0));

            //_animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weightLeftFoot);
            //_animator.SetIKPosition(AvatarIKGoal.LeftFoot, _leftLegPosition + new Vector3(0, 0.1f, 0));

            //_animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, weightRightFoot);
            //_animator.SetIKRotation(AvatarIKGoal.RightFoot, _rightLegRotation);

            //_animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, weightLeftFoot);
            //_animator.SetIKRotation(AvatarIKGoal.LeftFoot, _leftLegRotation);


            if (StateBot == StateBot.Detected)
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
                OnDieChange?.Invoke(this);
                _animator.SetInteger("Dead", 1);
            }
        }

        public void MovePoint(Vector3 point)
        {
            if (point == null)
                point = Patrol.GenericPoint(transform);
            Agent.SetDestination(point);
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
                    child.localScale = obj.localScale;
                }
                else
                    PlaceAndSetWeapons(obj, parent, child);
            }
        }

        #endregion
    }
}