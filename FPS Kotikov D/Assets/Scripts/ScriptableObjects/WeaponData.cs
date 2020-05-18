using UnityEngine;


namespace FPS_Kotikov_D
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "CreateScriptableObjects/WeaponData")]
    public sealed class WeaponData : ScriptableObject
    {


        #region Fields

        [Header("Weapon settings")]
        public GunType GunType;
        public Ammunition Ammunition;
        [SerializeField, Tooltip("Decoration effect for shooting")]
        internal GameObject ParticalShoot;
        [SerializeField]
        internal float BulletSpeed = 100f;
        [SerializeField, Tooltip("Delay between shoots")]
        internal float RechargeTime = 0.2f;
        [SerializeField, Tooltip("Reload delay")]
        internal float ReloadTime = 3f;
        [SerializeField, Tooltip("Start clips count")]
        internal int CurrentCountClip = 4;
        [SerializeField, Tooltip("Max count ammo in one clip")]
        internal int MaxCountAmmunition = 10;
        [SerializeField]
        internal int MaxCountClips = 10;
        [SerializeField, Tooltip("This weapon will avaliable to use for player if check it")]
        internal bool IsAvailableForPlayer = false;

        #endregion


    }
}