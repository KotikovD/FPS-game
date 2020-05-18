using UnityEngine;
using TMPro;


namespace FPS_Kotikov_D
{
    public class WeaponsUI : MonoBehaviour
    {


        #region Fields

        private const string RELOADING = "reload";

        private TextMeshProUGUI _textUI;
        private string _clipText;
        private string _ammoText;

        #endregion


        #region Properties

        public int AmmunitionText
        {
            set
            {
                _ammoText = value.ToString();
                DrawUI();
            }
        }

        public int ClipText
        {
            set
            {
                _clipText = value.ToString();
                DrawUI();
            }
        }

        #endregion


        #region UnityMethods

        public void Awake()
        {
            _textUI = transform.GetComponentInChildren<TextMeshProUGUI>();
        }

        #endregion


        #region Methods

        public void DrawUIReload()
        {
            if (_textUI == null) return;
            _textUI.text = RELOADING;
        }

        private void DrawUI()
        {
            _textUI.text = $"{_clipText}" + System.Environment.NewLine + $"{_ammoText}";
        }

        #endregion


    }
}