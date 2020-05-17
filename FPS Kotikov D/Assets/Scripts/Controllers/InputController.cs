using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


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
       // private bool _areHandsBusy = false;
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
          //  _areHandsBusy = _interaction.IsCatched;

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
                if (_interaction.IsCatched) return;
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
                if (_interaction.IsCatched)
                    _interaction.ReleaseObject();
                SelectWeapon(0);
                
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (_interaction.IsCatched)
                    _interaction.ReleaseObject();
                SelectWeapon(1);
            }



            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_interaction.IsCatched)
                {
                    _interaction.ReleaseObject();
                }
                else
                {
                    ServiceLocator.Resolve<PlayerController>().TryUse = true;
                    //if (!_interaction.ObjCanCollect)
                    //{
                        
                    //}
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
        private void SelectWeapon(int i)
        {
            ServiceLocator.Resolve<WeaponController>().Off();
            Weapons tempWeapon = Player.Weapons[i]; //todo инкапсулировать
            if (tempWeapon != null)
            {
               var weapon =  ServiceLocator.Resolve<PlayerController>().SwitchActiveWeapon(tempWeapon, true);
                ServiceLocator.Resolve<WeaponController>().On(weapon);
            }
        }

       


        #endregion


    }
}