using UnityEngine;


namespace FPS_Kotikov_D
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class AmmunitionClip : PickUps, ISerializable, IInteraction, ICanCollect, IShowMessage
    {


        #region Fields

        public int CountClips = 1;

        private const string EMPTY = "EMPTY";
        [Header("Ammo settings")]
        [SerializeField] private string _nameForMessage;
        [SerializeField] private AmmunitionType _typeAmunition;

        #endregion


        #region Properties

        public bool IsCanCollect
        {
            get;
            set;
        }

        #endregion


        #region Methods

        public void GetCollect()
        {
            if (IsCanCollect)
                foreach (Weapons weapon in Player.Weapons)
                {
                    if (weapon != null)
                        if (_typeAmunition.Equals(weapon.Ammunition.Type))
                        {
                            for (int i = 0; i < CountClips; i++)
                            {
                                weapon.AddClip(new Clip { CountAmmunition = weapon.MaxCountAmmunition });

                                if (weapon.CountClips == 1 && weapon.CurrentAmmunition == 0)
                                    if (weapon.enabled == true)
                                        weapon.ReloadClip();
                                    else
                                        weapon.SilanceReload();
                            }

                            DestroyAmmunitionClip();
                        }
                }
        }

        private void DestroyAmmunitionClip()
        {
            Destroy(gameObject);
        }

        public void Interaction<T>(T value = null) where T : class
        {
            IsCanCollect = CountClips == 0 ? false : true;
            RaiseUp();
            DeactiveIcon();
        }

        public void ShowMessage()
        {
            if (_nameForMessage != string.Empty)
                gameObject.name = _nameForMessage;

            if (CountClips == 0)
                gameObject.name = EMPTY;
            else
                gameObject.name = $"{gameObject.name} {CountClips}";
            ShowName();
            if (_hologramIcon != null && CountClips > 0 && !_isIconActive)
            {
                ActiveIcon();
                StartCoroutine(DeactiveIcon(_iconShowTime));
            }
        }

        #endregion


    }
}