using UnityEngine;


namespace FPS_Kotikov_D
{
    public sealed class WeaponController : BaseController, IExecute
    {


        #region Fields

        private Weapons _weapon;

        #endregion


        #region Methods

        public void Execute()
        {
            if (!IsActive) return;

            _weapon.MoveWeaponUI();
            _weapon.WeaponsUI.DrawUIclips(_weapon.CountClips);

            if (_weapon.IsReloading)
                _weapon.WeaponsUI.DrawUIAmmunitionReload();
            else
                _weapon.WeaponsUI.DrawUIammunition(_weapon.CurrentAmmunition);
        }

        public void Fire()
        {
            _weapon.Fire();
        }


        public void Switch(bool value)
        {
            _weapon.enabled = value;
            _weapon.gameObject.SetActive(value);
        }


        public override void On(params BaseObjectScene[] weapon)
        {
            if (IsActive) return;
            if (weapon.Length > 0)
                _weapon = weapon[0] as Weapons;
            if (_weapon == null) return;

            base.On(_weapon);
            Switch(true);

            _weapon.WeaponsUI.SetActive(true);
            _weapon.MoveWeaponUI();

        }

        public override void Off()
        {
            if (!IsActive) return;
            base.Off();
            Switch(false);
            _weapon.WeaponsUI.SetActive(false);

        }




        #endregion


    }
}