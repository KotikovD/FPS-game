using UnityEngine;


namespace FPS_Kotikov_D.Controller
{
    public sealed class WeaponController : BaseController, IExecute, IInitialization
    {


        #region Fields

        private Weapons _weapon;
        private PlayerController _playerController;

        #endregion


        #region Methods

        public void Initialization()
        {
            _playerController = ServiceLocator.Resolve<PlayerController>();
        }

        public void Execute()
        {
            if (!IsActive) return;
            if (_weapon == null) return;
            if (!_weapon.AvailableForPlayer) return;
            _weapon.WeaponRotation(_playerController.HitPoint);

#if UNITY_EDITOR
            Debug.DrawRay(_weapon.BulletSpawn.position, _weapon.BulletSpawn.forward *
                GameObject.FindObjectOfType<Player>().MaxViewDistance, Color.blue);
#endif
            if (_weapon.WeaponUI != null)
            {
                _weapon.WeaponUI.ClipText = _weapon.CountClips;

                if (_weapon.IsReloading)
                    _weapon.WeaponUI.DrawUIReload();
                else
                    _weapon.WeaponUI.AmmunitionText = _weapon.CurrentAmmunition;
            }
        }

        public void Fire()
        {
            _weapon.AnimFire();
        }

        public override void On(params BaseObjectScene[] weapon)
        {
            if (IsActive) return;
            if (weapon.Length > 0)
                _weapon = weapon[0] as Weapons;
            if (_weapon == null) return;
            if (!_weapon.AvailableForPlayer) return;

            base.On(_weapon);

            _weapon.AddUIToWeapon();
            _weapon.Switch(true);
      
        }

        public override void Off()
        {
            if (!IsActive) return;
            if (_weapon.IsReloading)
                _weapon.ForceFinishReload();
            base.Off();
            _weapon.Switch(false);
        }


        #endregion


    }
}