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


        #region Properties

        public Vector3 HitPoint => _hitPoint;
        public Weapons[] Weapons => _player.Weapons;

        #endregion


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

            _gameUI.PlayerHpText = _player.CurrentHp;
            ray = _mainCamera.ScreenPointToRay(_screenCenter);

#if UNITY_EDITOR           
            Debug.DrawRay(_mainCamera.transform.position, _mainCamera.transform.forward * _player.MaxViewDistance, Color.red);
#endif

            if (Physics.Raycast(ray, out var hit, _player.MaxViewDistance, _player.ViewLayers))
            {
                if (hit.distance < _player.MinViewDistance)
                    _hitPoint = _mainCamera.transform.position + ray.direction * _player.MinViewDistance;
                else
                    _hitPoint = hit.point;

                if (hit.distance < _player.InteractionDistance)
                {
                    var objIS = hit.collider.gameObject.GetComponent<IShowMessage>();
                    if (objIS != null)
                        objIS.ShowMessage();

                    if (TryUse)
                    {
                        var objII = hit.collider.gameObject;
                        var objChek = objII.GetComponent<IInteraction>();
                        if (objChek == null) return;

                        objChek.Interaction(_gameUI);

                        var rb = objII.GetComponent<Rigidbody>();
                        if (rb == null)
                            rb = objII.AddComponent<Rigidbody>();

                        var bc = objII.GetComponent<BoxCollider>();
                        if (bc == null)
                            bc = objII.AddComponent<BoxCollider>();

                        rb.useGravity = false;

                        _player.Interaction.MySpringJoint.connectedBody = rb;
                        _player.Interaction.MySpringJoint.connectedAnchor = bc.center;
                        _player.Interaction.MySpringJoint.spring = rb.mass * _player.Interaction.SpringJointForceMyltipler;
                    }
                }
            }
            else
            {
                GameUI.SetMessageBox = string.Empty;
                _hitPoint = _mainCamera.transform.position + ray.direction * _player.MaxViewDistance;
            }
        }

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

        #endregion


    }
}