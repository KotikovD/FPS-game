﻿using UnityEngine;


namespace FPS_Kotikov_D.Controller
{
    public sealed class WeaponController : BaseController, IExecute, IInitialization
    {


        #region Fields

        private Weapons _weapon;
        private WeaponsUI _weaponUI; // weapon UI is a part of weapon and must placed on a barrel
        private PlayerController _playerController;

        #endregion


        #region Methods

        public void Initialization()
        {
            _weaponUI = Object.FindObjectOfType<WeaponsUI>();
            _playerController  = ServiceLocator.Resolve<PlayerController>();
        }

        public void Execute()
        {
            if (!IsActive) return;
            _weapon.WeaponRotation(_playerController.HitPoint);

#if UNITY_EDITOR
            Debug.DrawRay(_weapon.BulletSpawn.position, _weapon.BulletSpawn.forward * 50, Color.red);
#endif

            _weaponUI.DrawUIclips(_weapon.CountClips);

            if (_weapon.IsReloading)
                _weaponUI.DrawUIAmmunitionReload();
            else
                _weaponUI.DrawUIammunition(_weapon.CurrentAmmunition);
        }

        public void Fire()
        {
            _weapon.Fire();
        }

        public override void On(params BaseObjectScene[] weapon)
        {
            if (IsActive) return;
            if (weapon.Length > 0)
                _weapon = weapon[0] as Weapons;
            if (_weapon == null) return;

            base.On(_weapon);

            _weaponUI.transform.SetParent(_weapon.WeaponUIplace.transform);
            _weaponUI.PlaceWeaponUI(_weapon.WeaponUIplace.transform);
            _weapon.Switch(true);

            _weaponUI.SetActive(true);

        }

        public override void Off()
        {
            if (!IsActive) return;
            base.Off();
            _weapon.Switch(false);
            _weaponUI.SetActive(false);
        }


        #endregion


    }
}