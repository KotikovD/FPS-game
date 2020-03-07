using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace FPS_Kotikov_D
{
    public class WeaponsUI : MonoBehaviour
    {


        #region Fields

        private const string RELOADING = "RELOAD";

        private TextMeshPro _ammunitionText;
        private TextMeshPro _clipText;
        private Image _screen;

        #endregion


        #region Properties

        public string AmmunitionReload
        {
            set
            {
                _ammunitionText.text = value;
            }
        }

        public int AmmunitionText
        {
            set
            {
                _ammunitionText.text = $"{value}";
            }
        }

        public int ClipText
        {
            set
            {
                _clipText.text = $"{value}";
            }
        }

        #endregion


        #region UnityMethods

        public void Awake()
        {
            _ammunitionText = GameObject.Find("AmmunitionTextUI").GetComponent<TextMeshPro>();
            _clipText = GameObject.Find("ClipsTextUI").GetComponent<TextMeshPro>();
            _screen = gameObject.GetComponentInChildren<Image>();
            SetActive(false);
        }

        #endregion


        #region Methods

        public void SetActive(bool value)
        {
            _ammunitionText.gameObject.SetActive(value);
            _clipText.gameObject.SetActive(value);
            _screen.gameObject.SetActive(value);
        }

        public void PlaceUI(Transform value)
        {
            gameObject.transform.position = value.position;
            gameObject.transform.rotation = value.rotation;
        }

        public void DrawUIclips(int value)
        {
            _clipText.text = $"{value}";
        }

        public void DrawUIammunition(int value)
        {
            _ammunitionText.text = $"{value}";
        }

        public void DrawUIAmmunitionReload()
        {
            _ammunitionText.text = RELOADING;
        }

        #endregion


    }
}