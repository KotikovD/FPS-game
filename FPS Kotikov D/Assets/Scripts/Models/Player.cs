using UnityEngine;


namespace FPS_Kotikov_D
{

    public class Player : BaseObjectScene, IHeal
    {


        #region Fields

        public float ViewDistance = 50.0f;
        public LayerMask ViewLayers;

        [SerializeField] private float _maxHp = 100.0f;
        [SerializeField] private float _currentHp = 40.0f;

        #endregion


        #region Properties

        public float CurrentHp => _currentHp;


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