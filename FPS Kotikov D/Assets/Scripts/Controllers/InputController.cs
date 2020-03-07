using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace FPS_Kotikov_D.Controller
{
    /// <summary>
    /// Read all inputs in programm 
    /// </summary>
    public class InputController : BaseController, IExecute
    {


        #region Fields

        private bool _isActiveFlashlight = false;

        #endregion


        #region Methods


        public void Execute()
        {
            if (!IsActive) return;

            if (CrossPlatformInputManager.GetButtonDown("FlashLight"))
            { 
                _isActiveFlashlight = !_isActiveFlashlight;
                if (_isActiveFlashlight)
                {
                    ServiceLocator.Resolve<FlashlightController>().On();
                }
                else
                {
                    ServiceLocator.Resolve<FlashlightController>().Off();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                ServiceLocator.Resolve<WeaponController>().Off();
            }


            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectWeapon(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectWeapon(1);
            }

            if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                if (ServiceLocator.Resolve<WeaponController>().IsActive)
                {
                    ServiceLocator.Resolve<WeaponController>().Fire();
                }
            }
        }



        /// <summary>
        /// Выбор оружия
        /// </summary>
        /// <param name="i">Номер оружия</param>
        private void SelectWeapon(int i)
        {
            ServiceLocator.Resolve<WeaponController>().Off();
            Weapons tempWeapon = ServiceLocator.Resolve<Inventory>().Weapons[i]; //todo инкапсулировать
            if (tempWeapon != null)
            {
                ServiceLocator.Resolve<WeaponController>().On(tempWeapon);
            }
        }


        #endregion


    }
}