using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using FPS_Kotikov_D.Data;

namespace FPS_Kotikov_D.Controller
{
    /// <summary>
    /// Read all inputs in programm 
    /// </summary>
    public class InputController : BaseController, IExecute, IInitialization
    {


        #region Fields

        private bool _isActiveFlashlight = false;
        private bool _isActivePockePC = false;
        private bool _isActiveGameMenu = false;
        private bool _areHandsBusy = false;
        private InteractionPoint _interaction;

        #endregion


        #region Methods

        public void Initialization()
        {
            _interaction = GameObject.FindObjectOfType<InteractionPoint>();
        }


        public void Execute()
        {
            if (!IsActive) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var gameMenu = ServiceLocatorMonoBehaviour.GetService<Reference>().GameMenu;

                _isActiveGameMenu = !_isActiveGameMenu;
                if (_isActiveGameMenu)
                {
                    gameMenu.ActiveGameMenu(true);
                }
                else
                {
                    gameMenu.ActiveGameMenu(false);
                }

            }


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

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (_areHandsBusy) return;
                _isActivePockePC = !_isActivePockePC;
                if (_isActivePockePC)
                {
                    ServiceLocator.Resolve<PocketPCController>().On();
                }
                else
                {
                    ServiceLocator.Resolve<PocketPCController>().Off();
                }
                
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                
                    ServiceLocator.Resolve<WeaponController>().Off();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (_areHandsBusy) return;
                SelectWeapon(0);
                
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (_areHandsBusy) return;
                SelectWeapon(1);
            }



            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!_areHandsBusy)
                {
                    _areHandsBusy = true;
                    ServiceLocator.Resolve<PlayerController>().TryUse = true;
                    ServiceLocator.Resolve<WeaponController>().Off();
                    ServiceLocator.Resolve<PocketPCController>().Off();
                }
                else
                {
                    _interaction.ReleaseObject();
                    _areHandsBusy = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                ServiceLocator.Resolve<PlayerController>().TryUse = false;
            }



            if (CrossPlatformInputManager.GetButton("Fire1"))
            {
                if (_interaction.IsCatched)
                {
                    _interaction.ThrowObject();
                    _areHandsBusy = false;
                }
                else
                {
                    if (ServiceLocator.Resolve<WeaponController>().IsActive)
                    {
                        ServiceLocator.Resolve<WeaponController>().Fire();
                    }
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
            Weapons tempWeapon = ServiceLocator.Resolve<PlayerController>().Weapons[i]; //todo инкапсулировать
            if (tempWeapon != null)
            {
               var weapon =  ServiceLocator.Resolve<PlayerController>().SwitchActiveWeapon(tempWeapon, true);
                ServiceLocator.Resolve<WeaponController>().On(weapon);
            }
        }

       


        #endregion


    }
}