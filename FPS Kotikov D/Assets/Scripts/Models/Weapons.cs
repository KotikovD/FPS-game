using UnityEngine;
using System.Collections.Generic;
using FPS_Kotikov_D.Controller;
using DG.Tweening;


namespace FPS_Kotikov_D
{
    public abstract class Weapons : BaseObjectScene
    {


        #region Fields

        private const float FIXDELAY = 2.0f;

        [HideInInspector]
        public Clip Clip;
        [HideInInspector]
        public bool IsReloading = false;
        [HideInInspector]
        public bool CanFire = true;
        [Header("Main settings")]
        public GunType GunType;
        public Ammunition Ammunition;

        protected Transform _bulletSpawn;
        protected Animator _animator;
        [SerializeField, Tooltip("Decoration effect for shooting")]
        protected GameObject ParticalShoot;
        [SerializeField]
        protected float _bulletSpeed = 100f;
        [SerializeField, Tooltip("Delay between shoots")]
        protected float _rechargeTime = 0.2f;
        [SerializeField, Tooltip("Reload delay")]
        protected float _reloadTime = 3f;
        [SerializeField, Tooltip("Start clips count")]
        protected int _countClip = 4;
        [SerializeField, Tooltip("Max count ammo in one clip")]
        private int _maxCountAmmunition = 10;
        [SerializeField]
        private int _maxCountClips = 10;
        [SerializeField, Tooltip("This weapon will avaliable to use for player if check it")]
        private bool _availableForPlayer = false;
        [Header("Camera shake animation (DOTweener)")]
        [SerializeField] private float _duration;
        [SerializeField, Range(0, 10)] private float _strength;
        [SerializeField] private int _vibrato;
        [SerializeField, Range (0f, 90f)] private float _randomness;
        private Transform _camera;
        private WeaponsUI _weaponUI;
        private Queue<Clip> _clips = new Queue<Clip>();
        private Transform _leftHandPosition;

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
            _bulletSpawn = transform.Find("BulletSpawn").transform;
            _camera = Camera.main.transform;

            if (_countClip > 0)
                for (var i = 0; i <= _countClip; i++)
                {
                    AddClip(new Clip { CountAmmunition = _maxCountAmmunition });
                }

            ReloadClip();
        }

        #endregion


        #region Metodths

        public abstract void Fire();

        public void AddUIToWeapon()
        {
            _weaponUI = transform.GetComponentInChildren<WeaponsUI>();
        }

        public void AnimFire()
        {
            if (!CanFire) return;
            if (IsReloading) return;
            _animator.SetBool("Shoot", true);



            Tweener tweener = DOTween.Shake
                (() => _camera.localPosition, pos => _camera.localPosition = pos,
                _duration, _strength, _vibrato, _randomness);
        }

        public void Switch(bool value)
        {
            if (value)
            {
                enabled = value;
                gameObject.SetActive(value);
                SetAnimtator();
            }
            else
            {
                _animator.SetBool("RemoveWeapon", true);
                gameObject.SetActive(false);
                enabled = false;
            }
        }

        protected void ReadyShoot()
        {
            CanFire = true;
            _animator.SetBool("Shoot", false);
        }

        public void AddClip(Clip clip)
        {
            if (CountClips >= _maxCountClips)
                return;
            _clips.Enqueue(clip);
        }

        /// <summary>
        /// Using while the weapon is enabled = false;
        /// </summary>
        public void SilanceReload()
        {
            if (CurrentAmmunition == 0 && CountClips == 1)
            {
                IsReloading = true;
                Invoke(nameof(ReloadIsFinish), 1f);
            }
            else if (CountClips == 0 && CurrentAmmunition == 0)
            {
                IsReloading = true;
            }
            else if (!IsReloading && CountClips > 0)
            {
                IsReloading = true;
                Invoke(nameof(ReloadIsFinish), _reloadTime);
            }
        }

        public void ReloadClip()
        {
            if (CurrentAmmunition == 0 && CountClips == 1)
            {
                IsReloading = true;
                Invoke(nameof(ReloadIsFinish), 1f);
                _animator.SetBool("IsReloading", IsReloading);
                _animator.SetBool("HaveNextClip", true);
            }
            else if (CountClips == 0 && CurrentAmmunition == 0)
            {
                IsReloading = true;
                _animator.SetBool("IsReloading", IsReloading);
                _animator.SetBool("HaveNextClip", false);
            }
            else if (!IsReloading && CountClips > 0)
            {
                IsReloading = true;
                _animator.SetBool("HaveNextClip", true);
                _animator.SetBool("IsReloading", IsReloading);
                Invoke(nameof(ReloadIsFinish), _reloadTime);
            }
        }

        /// <summary>
        /// Using if weapon Switch.Off while it reloading
        /// </summary>
        public void ForceFinishReload()
        {
            IsReloading = false;
            _animator.SetBool("IsReloading", IsReloading);
        }

        private void ReloadIsFinish()
        {
            IsReloading = false;
            Clip = _clips.Dequeue();
            if (enabled)
                _animator.SetBool("IsReloading", IsReloading);
        }

        public void WeaponRotation(Vector3 aim)
        {
            if (IsReloading) return;
            BulletSpawn.LookAt(aim);
            _animator.SetFloat("Magnitude", PlayerController.MoveMagnitude);
        }

        private void SetAnimtator()
        {
            _animator = GetComponent<Animator>();

            var delay = _reloadTime;
            if (delay - FIXDELAY > 0)
                delay = 1 / (delay - FIXDELAY);
            _animator.SetFloat("ReloadDelay", delay);

            // TODO remove magic from here
            var reCharge = _rechargeTime;
            if (reCharge > 0)
                reCharge = reCharge / 60 * 100 * 100;
            _animator.SetFloat("ShootRecharge", reCharge);
        }

        #endregion


    }
}