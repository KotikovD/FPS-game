using UnityEngine;


namespace FPS_Kotikov_D
{

    public abstract class Ammunition : BaseObjectScene
    {


        #region Fields

        public AmmunitionType Type = AmmunitionType.Bullet;

        [SerializeField] private float _timeToDestruct = 10f;
        [SerializeField] private float _baseDamage = 10f;
        [SerializeField] private float _lossOfDamageAtTime = 0.2f;
        protected float _currentDamage;


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
            InvokeRepeating(nameof(LossOfDamage), 0, 1);
        }

        #endregion


        #region Methods

        public void AddForce(Vector3 dir)
        {
            if (!Rigidbody) return;
            Rigidbody.AddForce(dir);
        }

        private void LossOfDamage()
        {
            _currentDamage -= _lossOfDamageAtTime;
            if (_currentDamage < 0)
                DestroyAmmunition();
        }

        protected void DestroyAmmunition(float timeToDestruct = 0)
        {
            Destroy(gameObject, timeToDestruct);
            CancelInvoke(nameof(LossOfDamage));
            // Вернуть в пул
        }

        #endregion


    }
}