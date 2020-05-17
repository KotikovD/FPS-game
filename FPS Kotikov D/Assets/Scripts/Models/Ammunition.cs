using UnityEngine;


namespace FPS_Kotikov_D
{

    public abstract class Ammunition : BaseObjectScene
    {


        #region Fields

        public AmmunitionType Type = AmmunitionType.Bullet;
        [HideInInspector] public Vector3 Direction;

        protected float _currentDamage;
        [SerializeField] protected float _timeToDestruct = 1f;
        [SerializeField] protected float _addForcePower = 5f;
        [SerializeField] private float _baseDamage = 10f;

        #endregion


        #region Fields

        public float BaseDamage => _baseDamage;
        public float TimeToDestruct => _timeToDestruct;

        #endregion


        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();
            _currentDamage = _baseDamage;
        }

        private void Start()
        {
            Destroy(InstanceObject, _timeToDestruct);
        }

        #endregion


        #region Methods




        protected void DestroyAmmunition(float timeToDestruct = 0)
        {
            Destroy(gameObject, timeToDestruct);
            // TODO Вернуть в пул
        }

        #endregion


    }
}