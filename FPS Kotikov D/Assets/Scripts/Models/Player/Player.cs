using UnityEngine;


namespace FPS_Kotikov_D
{

    public class Player : BaseObjectScene, IHeal, ISerializable
    {


        #region Fields

        public static SpringJoint Interaction;

        public float MaxViewDistance = 50.0f;
        public float InteractionDistance = 5.0f;
        public float MinViewDistance = 5.0f;
        public LayerMask ViewLayers;

        [SerializeField] private float _maxHp = 100.0f;
        [SerializeField] private float _currentHp = 40.0f;
        [SerializeField] private Transform _weaponPlace;
        private Weapons[] _weapons;

        #endregion


        #region Properties

        public float CurrentHp
        {
            get { return _currentHp; }
            set { _currentHp = value; }
        }

        public Transform WeaponPlace
        {
            get { return _weaponPlace; }
        }

        public Weapons[] Weapons
        {
            get { return _weapons; }
        }

        #endregion


        #region Methodss


        public bool Heal(float heal)
        {
            if (_currentHp < _maxHp)
            {
                _currentHp += heal;
                if (_currentHp > _maxHp)
                    _currentHp = _maxHp;
                return true;
            }
            return false;
        }

        public void AddWeapons()
        {
            _weapons = new Weapons[ServiceLocator.Resolve<Inventory>().Length];
            var weapons = ServiceLocator.Resolve<Inventory>().Weapons;
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i] != null)
                {
                    var localWeapon = Instantiate(weapons[i], _weaponPlace.position, _weaponPlace.transform.rotation);
                    localWeapon.transform.SetParent(_weaponPlace.transform);
                    _weapons[i] = localWeapon;
                }
            }
        }


        #endregion


    }
}