using UnityEngine;
using System.Collections.Generic;


namespace FPS_Kotikov_D
{
    public abstract class Weapons : BaseObjectScene
    {


        #region Fields

        public Ammunition Ammunition;
        public Clip Clip;
        public bool IsReloading = false;
        public bool CanFire = true;
        

        [SerializeField, Tooltip("Weapon UI placer")]
        private Transform _weaponUIplace;
        [SerializeField, Tooltip("Force of shoot")]
        protected float _force = 500;
        [SerializeField, Tooltip("Delay between shoots")]
        protected float _rechargeTime = 0.2f;
        [SerializeField, Tooltip("Reload delay")]
        protected float _reloadTime = 3f;
        [SerializeField, Tooltip("Start clips count")]
        protected int _countClip = 4;
        [SerializeField, Tooltip("Bullet spawn place")]
        protected Transform _bulletSpawn; 
        [SerializeField, Tooltip("Max count ammo in one clip")]
        private int _maxCountAmmunition = 10;
        [SerializeField]
        private int _maxCountClips = 10;
        private Queue<Clip> _clips = new Queue<Clip>();

        #endregion


        #region Properties

        public int CountClips
        {
            get { return _clips.Count; }
        }

        public int CurrentAmmunition
        {
            get { return Clip.CountAmmunition; }
            set { Clip.CountAmmunition = value; }
        }

        public int MaxCountAmmunition
        {
            get { return _maxCountAmmunition; }
        }

        public int MaxCountClips
        {
            get { return _maxCountClips; }
        }


        public Transform WeaponUIplace
        {
            get { return _weaponUIplace; }
            private set { _weaponUIplace = value; }
        }

        public Transform BulletSpawn => _bulletSpawn;

        #endregion


        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();
            
        }

        private void Start()
        {
            

            for (var i = 0; i <= _countClip; i++)
            {
                AddClip(new Clip { CountAmmunition = _maxCountAmmunition });
            }

            ReloadClip();
        }

        #endregion

        #region Metodths

        public abstract void Fire();

        public void Switch(bool value)
        {
            enabled = value;
            gameObject.SetActive(value);
        }

        protected void ReadyShoot()
        {
            CanFire = true;
        }

        public void AddClip(Clip clip)
        {
            _clips.Enqueue(clip);
        }

        public void ReloadClip()
        {
            if (CountClips <= 0) return;
            if (CountClips >= _maxCountClips) return;
            IsReloading = true;
            Invoke(nameof(ReloadIsFinish), _reloadTime);
        }

        private void ReloadIsFinish()
        {
            IsReloading = false;
            Clip = _clips.Dequeue();
        }

        public void WeaponRotation(Vector3 aim)
        {
            transform.LookAt(aim);
            BulletSpawn.LookAt(aim);
        }

        #endregion


    }
}