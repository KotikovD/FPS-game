using UnityEngine;


namespace FPS_Kotikov_D
{

    public class FlashlightModel : BaseObjectScene
    {


        #region Fields

        [SerializeField] private float _movingSpeed = 11;
        [SerializeField] private float _batteryChargeMax = 10;
        private Transform _goFollow;
        private Light _light;
        

        #endregion


        #region Properties

        public float BatteryChargeCurrent { get; private set; }
        public float BatteryChargeMax { get; private set; }

        #endregion


        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();
            _light = GetComponent<Light>();
            _goFollow = Camera.main.transform;
            BatteryChargeCurrent = _batteryChargeMax;
            BatteryChargeMax = _batteryChargeMax;
        }

        #endregion


        #region Methods

        public void Switch(bool value)
        {
            _light.enabled = value;
        }

        public void LightRotation()
        {
            GetTransform.position = _goFollow.position;
            GetTransform.rotation = Quaternion.Lerp(GetTransform.rotation,
                _goFollow.rotation, _movingSpeed * Time.deltaTime);
        }

        public bool EditBatteryCharge()
        {
            
                if (BatteryChargeCurrent > 0)
                { 
                    BatteryChargeCurrent -= Time.deltaTime;
                    return true;
                }
                else 
                return false;
        }

        public bool ChargingBattery()
        {
            if (BatteryChargeCurrent < _batteryChargeMax)
            {
                BatteryChargeCurrent += Time.deltaTime;
                return true;

            }
            else
                return false;
        }

    #endregion


}
}