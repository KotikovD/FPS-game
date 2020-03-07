using UnityEngine;
using UnityEngine.UI;


namespace FPS_Kotikov_D
{
    public class FlashLightUI : MonoBehaviour
    {


        #region Fields

        private Text _text;
        private Image _imageBar;
        private Color _fullChargeColor = Color.white;
        private Color _emptyChargeColor = Color.red;

        #endregion


        #region Properties

        public float Text
        {
           set {
                _text.text = $"{value:0.0}";
               }
        }

        /// <summary>
        /// Set correct values from 0 to 1
        /// </summary>
        public float ImageBar
        {
            set {
                _imageBar.fillAmount = value;
                _imageBar.color = LerpColor(value);
                }
        }

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _text = GameObject.Find("BatteryTextLevel").GetComponent<Text>();
            _imageBar = GameObject.Find("BatteryImageLevelBar").GetComponent<Image>();
        }

        #endregion


        #region Methods

        public void SetActive(bool value)
        {
            _text.gameObject.SetActive(value);
            _imageBar.gameObject.SetActive(value);
        }

        private Color LerpColor (float value)
        {
            return Color.Lerp(_emptyChargeColor, _fullChargeColor, value);
        }

        #endregion


    }
}