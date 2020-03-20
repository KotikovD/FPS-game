using System.Collections;
using UnityEngine;


namespace FPS_Kotikov_D
{
    public class PocketPC : BaseObjectScene
    {


        #region Fileds

        [SerializeField] private PocketPCUI[] _pages;

        [SerializeField, Header("Set boot screen index in massive")]
        private int _bootScreen = 0;
        [SerializeField]
        private float _bootTime = 2.0f;
        [SerializeField, Header("Set first screen after boot index in massive")]
        private int _firstPage = 1;

        #endregion


        #region Methods

        public void Switch(bool value)
        {
            enabled = value;
            gameObject.SetActive(value);
        }

        public void PocketPCBoot()
        {
            StartCoroutine(Boot());
        }

        public IEnumerator Boot()
        {
            PageActive(_pages[_bootScreen]);

            yield return new WaitForSeconds(_bootTime);

            PageActive(_pages[_firstPage]);
            StopAllCoroutines();
        }

        public void PageActive(PocketPCUI activePage)
        {
            foreach (var page in _pages)
            {
                if (activePage.Equals(page))
                    page.Switch(true);
                else
                    page.Switch(false);
            }
        }

        #endregion


    }
}