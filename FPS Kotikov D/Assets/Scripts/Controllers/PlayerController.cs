using UnityEngine;


namespace FPS_Kotikov_D.Controller
{

    public class PlayerController : BaseController, IExecute, IInitialization
    {
        #region Fields

        const int TWO = 2;

        public bool TryUse = false;

        private Vector3 _hitPoint;
        private GameUI _gameUI;
        private Player _player;
        private Camera _mainCamera;
        private Vector2 _screenCenter;
        private Ray ray;


        #endregion

        public Vector3 HitPoint => _hitPoint;
        public Weapons[] Weapons => _player.Weapons;

        #region Methods

        public void Initialization()
        {
            _player = Object.FindObjectOfType<Player>();
            _player.AddWeapons();
            _gameUI = Object.FindObjectOfType<GameUI>();
            _mainCamera = Camera.main;
            _screenCenter = new Vector2(Screen.width / TWO, Screen.height / TWO);

            On();
        }

        public void Execute()
        {
            if (!IsActive) return;
            if (!_mainCamera) return;
            //Debug.Log("AngularSpeed" + _player.Rigidbody.angularVelocity.magnitude);
            _gameUI.PlayerHpText = _player.CurrentHp;

            ray = _mainCamera.ScreenPointToRay(_screenCenter);

#if UNITY_EDITOR           
            Debug.DrawRay(_mainCamera.transform.position, _mainCamera.transform.forward * _player.MaxViewDistance, Color.red);
#endif

            if (Physics.Raycast(ray, out var hit, _player.InteractionDistance, _player.ViewLayers))
            {
                if (hit.distance < _player.MinViewDistance)
                    _hitPoint = _mainCamera.transform.position + ray.direction * _player.MinViewDistance;
                else
                    _hitPoint = hit.point;

                var objIS = hit.collider.gameObject.GetComponent<IShowMessage>();
                if (objIS != null)
                    objIS.ShowMessage();



                if (TryUse)
                {
                    var objII = hit.collider.gameObject.GetComponent<IInteraction>();
                    if (objII != null)
                        objII.Interaction(_gameUI);
                }
            }
            else
            {
                GameUI.SetMessageBox = string.Empty;
                _hitPoint = _mainCamera.transform.position + ray.direction * _player.MaxViewDistance;
            }
        }

        #endregion


        public Weapons SwitchActiveWeapon(Weapons enterWeapon, bool active)
        {
            foreach (var weapon in _player.Weapons)
            {
                if (!weapon.Equals(enterWeapon)) continue;

                weapon.AvailableForPlayer = active;
                Debug.Log(weapon.Equals(enterWeapon));
                return weapon;
            }
            return default;
        }

    }

}