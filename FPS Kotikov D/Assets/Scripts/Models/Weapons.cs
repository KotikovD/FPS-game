using UnityEngine;
using System.Collections.Generic;
using FPS_Kotikov_D.Controller;
using System;

namespace FPS_Kotikov_D
{
    public abstract class Weapons : BaseObjectScene
    {


        #region Fields

        private const float FIXDELAY = 2.0f;

        public event Action Shoot;
        [HideInInspector] public WeaponsUI WeaponUI;
        [HideInInspector] public Clip Clip;
        [HideInInspector] public bool IsReloading = false;
        [HideInInspector] public bool CanFire = true;
        public WeaponData Data;

        protected Transform _bulletSpawn;
        protected Animator _animator;

        private Queue<Clip> _clips = new Queue<Clip>();

        #endregion


        #region Properties

        public int MaxCountAmmunition => Data.MaxCountAmmunition;
        public int MaxCountClips => Data.MaxCountClips;
        public Transform BulletSpawn => _bulletSpawn;
        public GunType GunType => Data.GunType;
        public Ammunition Ammunition => Data.Ammunition;
        public Transform LeftHandPosition { get; private set; }

        public bool AvailableForPlayer
        {
            get { return Data.IsAvailableForPlayer; }
            set { Data.IsAvailableForPlayer = value; }
        }

        public int CountClips
        {
            get { return _clips.Count; }
            set
            {
                for (int i = 0; i < value; i++)
                    AddClip(new Clip { CountAmmunition = Data.MaxCountAmmunition });
            }
        }

        public int CurrentAmmunition
        {
            get { return Clip.CountAmmunition; }
            set { Clip.CountAmmunition = value; }
        }

        #endregion


        #region UnityMethods

        private void Start()
        {
            LeftHandPosition = transform.Find(StringKeeper.LeftHandPosition).transform;
            _bulletSpawn = transform.Find(StringKeeper.BulletSpawn).transform;
            if (Data.CurrentCountClip > 0)
            {
                for (var i = 0; i <= Data.CurrentCountClip; i++)
                {
                    AddClip(new Clip { CountAmmunition = Data.MaxCountAmmunition });
                }
            }
            ReloadClip();
        }

        #endregion


        #region Metodths

        public virtual void Fire(Vector3 hitPoint)
        {
            _animator.SetBool("Shoot", true);
            if (Shoot != null)
                Shoot.Invoke();
        }

        public void AddUIToWeapon()
        {
            WeaponUI = transform.GetComponentInChildren<WeaponsUI>();
        }

        //public void AnimFire()
        //{
        //    if (!CanFire) return;
        //    if (IsReloading) return;
        //    _animator.SetBool("Shoot", true);

        //    Tweener tweener = DOTween.Shake
        //        (() => _camera.localPosition, pos => _camera.localPosition = pos,
        //        _duration, _strength, _vibrato, _randomness);
        //}

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
            if (CountClips >= Data.MaxCountClips)
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
                Invoke(nameof(ReloadIsFinish), Data.ReloadTime);
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
                _animator.SetBool("NoAmmo", false);
            }
            else if (CountClips == 0 && CurrentAmmunition == 0)
            {
                IsReloading = true;
                _animator.SetBool("IsReloading", IsReloading);
                _animator.SetBool("HaveNextClip", false);
                _animator.SetBool("NoAmmo", true);
            }
            else if (!IsReloading && CountClips > 0)
            {
                IsReloading = true;
                _animator.SetBool("HaveNextClip", true);
                _animator.SetBool("NoAmmo", false);
                _animator.SetBool("IsReloading", IsReloading);
                Invoke(nameof(ReloadIsFinish), Data.ReloadTime);
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

            var delay = Data.ReloadTime;
            if (delay - FIXDELAY > 0)
                delay = 1 / (delay - FIXDELAY);
            _animator.SetFloat("ReloadDelay", delay);

            // TODO remove magic from here
            var reCharge = Data.RechargeTime;
            if (reCharge > 0)
                reCharge = reCharge / 60 * 100 * 100;
            _animator.SetFloat("ShootRecharge", reCharge);
        }



        #endregion


    }
}