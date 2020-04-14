using UnityEngine;
using System.Collections.Generic;


namespace FPS_Kotikov_D
{
    public abstract class Weapons : BaseObjectScene
    {


        #region Fields

        public GunType GunType;
        public Ammunition Ammunition;

        public Clip Clip;
        public bool IsReloading = false;
        public bool CanFire = true;

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
        [SerializeField]
        private bool _availableForPlayer = false;
        private WeaponsUI _weaponUI;
        private Queue<Clip> _clips = new Queue<Clip>();
        private Transform _leftHandPosition;
        protected Animator _animator;

        #endregion


        #region Properties

        public WeaponsUI WeaponUI
        {
            get { return _weaponUI; }
            set { _weaponUI = value; }
        }

        public bool AvailableForPlayer
        {
            get { return _availableForPlayer; }
            set { _availableForPlayer = value; }
        }

        public int CountClips
        {
            get { return _clips.Count; }
            set
            {
                for (int i = 0; i < value; i++)
                    AddClip(new Clip { CountAmmunition = _maxCountAmmunition });
            }
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

        public Transform BulletSpawn => _bulletSpawn;

        public Transform LeftHandPosition
        {
            get { return _leftHandPosition; }
            private set { _leftHandPosition = value; }
        }

        #endregion


        #region UnityMethods

        private void Start()
        {
            _leftHandPosition = transform.Find("LeftHandPosition").transform;
            _animator = GetComponent<Animator>();
            SetAnimtator();
            for (var i = 0; i <= _countClip; i++)
            {
                AddClip(new Clip { CountAmmunition = _maxCountAmmunition });
            }

            ReloadClip();
        }

        #endregion


        #region Metodths

        public void AddUIToWeapon()
        {
            _weaponUI = transform.GetComponentInChildren<WeaponsUI>();
        }

        public abstract void Fire();

        public void AnimFire()
        {
            if (!CanFire) return;
            if (IsReloading) return;
            _animator.SetBool("Shoot", true);
        }

        public void Switch(bool value)
        {
            enabled = value;
            gameObject.SetActive(value);
            SetAnimtator();
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
            if (IsReloading) return;

            IsReloading = true;
            _animator.SetBool("IsReloading", IsReloading);
            Invoke(nameof(ReloadIsFinish), _reloadTime);
        }

        private void ReloadIsFinish()
        {
            IsReloading = false;
            Clip = _clips.Dequeue();
            _animator.SetBool("IsReloading", IsReloading);
        }

        public void WeaponRotation(Vector3 aim)
        {
            if (IsReloading) return;
            //transform.LookAt(aim);
            BulletSpawn.LookAt(aim);
        }

        private void SetAnimtator()
        {
            if (_animator != null)
            {
                _animator.SetFloat("ShootRecharge", _rechargeTime);
                _animator.SetFloat("ReloadDelay", _reloadTime);
            }
        }

        #endregion


    }
}