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



        #endregion


        #region Properties

        public float CurrentHp
        {
            get { return _currentHp; }
            set { _currentHp = value; }
        }

        #endregion


        #region Methods

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


        #endregion


    }
}