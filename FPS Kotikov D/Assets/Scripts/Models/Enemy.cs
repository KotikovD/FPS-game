using UnityEngine;


namespace FPS_Kotikov_D
{
    public class Enemy : BaseObjectScene, ISetDamage
    {


        #region Fields

        [SerializeField]
        protected float _maxHealth = 100;

        [SerializeField]
        protected float _currentHealth = 100;
        [SerializeField]
        private float _timeToDestroy = 10.0f;

        private bool IsDead = false;
        

        #endregion


        #region Properties

        public float MaxHealth
        {
            get { return _maxHealth; }
        }

        public float CurrentHealth => _currentHealth;
        //{
        //    get { return _currentHealth; }
        //    set { _currentHealth = value; }
        //}

        #endregion


        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();
        }

        #endregion

        #region Metodths

       // public abstract void Fire();

        public void SetDamage(InfoCollision info)
        {
            if (IsDead) return;
            Debug.Log(info.Damage);
            if (_currentHealth > 0)
                _currentHealth -= info.Damage;
            
            if (_currentHealth <= 0)
            {
                
                Die();
                IsDead = true;
            }
        }

        public virtual void Die()
        {
            if (!TryGetComponent<Rigidbody>(out _))
            {
                gameObject.AddComponent<Rigidbody>();
            }
            Destroy(gameObject, _timeToDestroy);
        }

        #endregion


    }
}   