﻿using UnityEngine;


namespace FPS_Kotikov_D.Controller
{

    public class FlashlightController : BaseController, IExecute, IInitialization
    {


        #region Fields

        private FlashlightModel _light;
        private GameUI _gameUI;

        #endregion


        #region Methods

        public void Initialization()
        {
            _light = Object.FindObjectOfType<FlashlightModel>();
            _gameUI = Object.FindObjectOfType<GameUI>();
            _gameUI.FlashlightChargeText = _light.BatteryChargeCurrent;
           
            SetActiveFlashlight(false);
        }

        public void Execute()
        {
            if (IsActive)
            {
                CheckBattery();
                _light.LightRotation();
            }
            else
            {
                _light.ChargingBattery();
                DrawUIBattery();
            } 
        }

        #endregion


        #region LifecycleMethods

        private void CheckBattery()
        {
            

            if (IsActive && _light.EditBatteryCharge())
            {
                _light.EditBatteryCharge();
            }
            else
            {
                Off();
            }
            DrawUIBattery();
        }

        private void DrawUIBattery()
        {
            _gameUI.FlashlightChargeText = _light.BatteryChargeCurrent;
            _gameUI.FlashlightImageBar = _light.BatteryChargeCurrent / _light.BatteryChargeMax;
        }

        #endregion


        #region Methods

        private void SetActiveFlashlight(bool value)
        {
            _light.Switch(value);   
        }

        public override void On()
        {
            if (IsActive) return;     
            base.On();
            SetActiveFlashlight(true);
        }

        public override void Off()
        {
            if (!IsActive) return;   
            base.Off();
            SetActiveFlashlight(false);
        }

        #endregion


    }
}