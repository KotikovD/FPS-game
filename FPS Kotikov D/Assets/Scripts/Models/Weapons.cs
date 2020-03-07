using UnityEngine;
using System.Collections.Generic;


namespace FPS_Kotikov_D
{
    public abstract class Weapons : BaseObjectScene
    {


        #region Fields

        public Ammunition Ammunition;
        public Clip Clip;
        public int CountClip => _clips.Count;
        public bool IsReloading = false;
        public bool CanFire = true;
        

        [SerializeField, Tooltip("Weapon UI placer")]
        private Transform _weapoonUIplace;
        [SerializeField, Tooltip("Сила выстрела")]
        protected float _force = 500;
        [SerializeField, Tooltip("Время задержки между выстрелами")]
        protected float _rechargeTime = 0.2f;
        [SerializeField, Tooltip("Время задержки на перезарядку")]
        protected float _reloadTime = 3f;
        [SerializeField, Tooltip("Start clips count")]
        protected int _countClip = 4;
        [SerializeField, Tooltip("Bullet spawn place")]
        protected Transform _bulletSpawn;
        [SerializeField, Tooltip("Max count ammo in one clip")]

        private int _maxCountAmmunition = 10;
        private Queue<Clip> _clips = new Queue<Clip>();
        private WeaponsUI _weaponsUI;  //У меня интерфейс оружия - часть оружия, экран находящийся на стволе

        #endregion


        #region Properties

        public int CountClips
        {
            get { return _clips.Count; }
        }

        public int CurrentAmmunition
        {
            get { return Clip.CountAmmunition; }
            set { Clip.CountAmmunition = value; }
        }

        public WeaponsUI WeaponsUI => _weaponsUI;


        #endregion


        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();
            _weaponsUI = GameObject.Find("WeaponsUI").GetComponent<WeaponsUI>();
        }

        private void Start()
        {
            for (var i = 0; i <= _countClip; i++)
            {
                AddClip(new Clip { CountAmmunition = _maxCountAmmunition });
            }

            ReloadClip();
        }

        #endregion

        #region Metodths

        public abstract void Fire();

        protected void ReadyShoot()
        {
            CanFire = true;
        }

        protected void AddClip(Clip clip)
        {
            _clips.Enqueue(clip);
        }

        public void ReloadClip()
        {
            if (CountClip <= 0) return;
            IsReloading = true;
            Invoke("ReloadIsFinish", _reloadTime); 
        }

        private void ReloadIsFinish()
        {
            IsReloading = false;
            Clip = _clips.Dequeue();
        }

       public void MoveWeaponUI()
        {
            _weaponsUI.PlaceUI(_weapoonUIplace);
        }

        #endregion


    }
}   