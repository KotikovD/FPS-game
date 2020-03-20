using UnityEngine;
using UnityEngine.UI;


namespace FPS_Kotikov_D
{
    public class GameUI : MonoBehaviour
    {


        #region Fields

        private Text _textFlashlightCharge;
        private Image _imageFlashlightBar;
        private Color _fullChargeColor = Color.white;
        private Color _emptyChargeColor = Color.red;
        private Text _playerHp;
        private Text _iSee;

        #endregion


        #region Properties

        public float FlashlightChargeText
        {
            set
            {
                _textFlashlightCharge.text = $"{value:0.0}";
            }
        }

        /// <summary>
        /// Set correct values from 0 to 1
        /// </summary>
        public float FlashlightImageBar
        {
            set
            {
                _imageFlashlightBar.fillAmount = value;
                _imageFlashlightBar.color = LerpColor(value);
            }
        }

        public float PlayerHpText
        {
            set
            {
                _playerHp.text = $"HP: {value:0.0}";
            }
        }

        public string ISee
        {
            set
            {
                _iSee.text = value;
            }
        }

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _textFlashlightCharge = GameObject.Find("BatteryTextLevel").GetComponent<Text>();
            _imageFlashlightBar = GameObject.Find("BatteryImageLevelBar").GetComponent<Image>();
            _playerHp = GameObject.Find("PlayerHpText").GetComponent<Text>();
            _iSee = GameObject.Find("SayNameText").GetComponent<Text>();
        }

        #endregion


        #region Methods

        public void SetActive(bool value)
        {
            _textFlashlightCharge.gameObject.SetActive(value);
            _imageFlashlightBar.gameObject.SetActive(value);
        }

        private Color LerpColor(float value)
        {
            return Color.Lerp(_emptyChargeColor, _fullChargeColor, value);
        }

        #endregion


    }
}