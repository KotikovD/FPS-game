using DG.Tweening;
using UnityEngine;


namespace FPS_Kotikov_D
{

    public class Player : BaseObjectScene, IHeal, ISerializable
    {


        #region Fields

        [HideInInspector] public InteractionPoint Interaction;
        [Header("Aim settings")]
        public float MaxViewDistance = 50.0f;
        public float MinViewDistance = 5.0f;
        [Header("Interaction settings")]
        public float InteractionDistance = 5.0f;
        public LayerMask ViewLayers;
        
        [Header("Player's health")]
        [SerializeField] private static float _maxHp = 100.0f;
        [SerializeField] private static float _currentHp = 40.0f;
        [Header("Add for camera shake effect (optional)")]
        [SerializeField] private CameraShakeSO _shaker;
        private Transform _camera;

        #endregion


        #region Properties

        public static float CurrentHp
        {
            get { return _currentHp; }
            set { _currentHp = value; }
        }

        public static float MaxHp
        {
            get { return _maxHp; }
            set { _maxHp = value; }
        }

        public Transform WeaponPlace { get; private set; }
      
        public static Weapons[] Weapons { get; private set; }

        #endregion


        #region UnityMethods

        protected void Start()
        {
            WeaponPlace = GetComponentInChildren<WeaponPosition>().transform;
            Interaction = GetComponentInChildren<InteractionPoint>();
            _camera = Camera.main.transform;
        }

        //private void OnEnable()
        //{
        //    ServiceLocator.Resolve<WeaponController>().Shoot += CameraShake;
        //}

        //private void OnDisable()
        //{
        //    ServiceLocator.Resolve<WeaponController>().Shoot -= CameraShake;
        //}

        #endregion


        #region Methods

        public bool Heal(float heal = 0f)
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
            Weapons = new Weapons[ServiceLocator.Resolve<Inventory>().Length];
            var weapons = ServiceLocator.Resolve<Inventory>().Weapons;
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i] != null)
                {
                    var localWeapon = Instantiate(weapons[i], WeaponPlace.position, WeaponPlace.transform.rotation);
                    localWeapon.transform.SetParent(WeaponPlace.transform);
                    Weapons[i] = localWeapon;
                }
            }
        }

        public void CameraShake()
        {
            if (_shaker == null) return;
            Tweener tweener = DOTween.Shake
                (() => _camera.transform.localPosition, pos => _camera.transform.localPosition = pos,
                _shaker.Duration, _shaker.Strength, _shaker.Vibrato, _shaker.Randomness);
        }

        #endregion


    }
}