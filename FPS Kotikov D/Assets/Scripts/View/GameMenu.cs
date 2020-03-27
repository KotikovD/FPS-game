using UnityEngine;
using FPS_Kotikov_D.Data;
using UnityStandardAssets.Characters.FirstPerson;

namespace FPS_Kotikov_D
{
    public class GameMenu : MonoBehaviour
    {

        private void Awake()
        {
            ActiveGameMenu(false);
        }

        public void Load()
        {
            ServiceLocator.Resolve<SaveDataRepository>().Load();
        }

        public void Save()
        {
            ServiceLocator.Resolve<SaveDataRepository>().Save();
        }

        public void ActiveGameMenu(bool value)
        {
            gameObject.SetActive(value);
            Time.timeScale = value == true ? 0 : 1;
            MouseLook.LockCameraView = !value;
        }

    }
}