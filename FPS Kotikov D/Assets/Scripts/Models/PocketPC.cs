using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FPS_Kotikov_D
{
    public class PocketPC : BaseObjectScene
    {


        public PocketPCUI[] Pages;

        [SerializeField, Header("Set boot screen index in massive")]
        private int _bootScreen = 0;
        [SerializeField, Header("Set first screen after boot index in massive")]
        private int _firstPage = 1;
        [SerializeField]
        private float _bootTime = 2.0f;

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
            PageActive(Pages[_bootScreen]);

            yield return new WaitForSeconds(_bootTime);

            PageActive(Pages[_firstPage]);
            StopAllCoroutines();
        }

        public void PageActive(PocketPCUI activePage)
        {
            foreach (var page in Pages)
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