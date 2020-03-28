using UnityEngine;

namespace FPS_Kotikov_D
{
    public sealed class Inventory : BaseController, IInitialization
    {


        #region Fields

        private Weapons[] _weapons = new Weapons[5];

        #endregion


        #region Properies

        public Weapons[] Weapons
        {
            get { return _weapons; }
            private set { _weapons = value; }

        }

        public int Length
        {
            get { return _weapons.Length; }
        }

        #endregion


        #region Metodths

        public void Initialization()
        {
            _weapons = GameObject.FindObjectOfType<Player>().GetComponentsInChildren<Weapons>();

            foreach (var weapon in Weapons)
            {
                weapon.enabled = false;
                weapon.gameObject.SetActive(false);
            }
        }

        #endregion
    }

}
