using UnityEngine;


namespace FPS_Kotikov_D.Controller
{

    public class PlayerController : BaseController, IExecute, IInitialization
    {
        #region Fields

        const int TWO = 2;

        private Vector3 _hitPoint;
        private GameUI _gameUI;
        private Player _player;
        private Camera _mainCamera;
        private Vector2 _screenCenter;
        private float _minViewDistance = 5.0f;
        private Ray ray;

        #endregion

        public Vector3 HitPoint => _hitPoint;

        #region Methods

        public void Initialization()
        {
            _player = Object.FindObjectOfType<Player>();
            _gameUI = Object.FindObjectOfType<GameUI>();
            _mainCamera = Camera.main;
            _screenCenter = new Vector2(Screen.width / TWO, Screen.height / TWO);
            On();
        }

        public void Execute()
        {
            if (!IsActive) return;
            _gameUI.PlayerHpText = _player.CurrentHp;

            ray = _mainCamera.ScreenPointToRay(_screenCenter);
#if UNITY_EDITOR
            
            Debug.DrawRay(_mainCamera.transform.position, _mainCamera.transform.forward * _player.ViewDistance, Color.red);
         
#endif
            if (Physics.Raycast(ray, out var hit, _player.ViewDistance, _player.ViewLayers))
            {
                if (hit.distance < _minViewDistance)
                    _hitPoint = _mainCamera.transform.position + ray.direction * _minViewDistance;
                else
                    _hitPoint = hit.point;
                
      

                var obj = hit.collider.gameObject.GetComponent<IViewObject>().ViewObject();
                if (obj != null)
                {
                    _gameUI.ISee = obj;
                }
               
            }
            else
            {
                _gameUI.ISee = string.Empty;
                _hitPoint = _mainCamera.transform.position + ray.direction * _player.ViewDistance;
                
            }


        }

        #endregion


    }

}