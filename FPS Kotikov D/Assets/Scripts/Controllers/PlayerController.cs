using UnityEngine;


namespace FPS_Kotikov_D.Controller
{

    public class PlayerController : BaseController, IExecute, IInitialization
    {
        #region Fields

        private GameUI _gameUI;
        private Player _player;

        #endregion


        #region Methods

        public void Initialization()
        {
            _player = Object.FindObjectOfType<Player>();
            _gameUI = Object.FindObjectOfType<GameUI>();
        }

        public void Execute()
        {
            _gameUI.PlayerHpText = _player.CurrentHp;
        }

        #endregion


    }

}