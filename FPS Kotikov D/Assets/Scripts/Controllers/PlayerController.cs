using UnityEngine;


namespace FPS_Kotikov_D.Controller
{
    public class PlayerController : BaseController, IExecute, IInitialization
    {

        #region Fields

        const int TWO = 2;

        public static RaycastHit Hit;
        public bool TryUse = false;

        private Vector3 _hitPoint;
        private GameUI _gameUI;
        private Player _player;
        private Camera _mainCamera;
        private Vector2 _screenCenter;
        private Ray ray;
        private CharacterController _charController;
        private static float _moveMagnitude;

        #endregion


        #region Properties

        public Vector3 HitPoint => _hitPoint;
        public Player Player => _player;
        public static float MoveMagnitude => _moveMagnitude;

        #endregion


        #region Methods

        public void Initialization()
        {
            _player = Object.FindObjectOfType<Player>();
            _charController = _player.GetComponent<CharacterController>();
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
            _moveMagnitude = _charController.velocity.magnitude;
            _gameUI.PlayerHpText = Player.CurrentHp;
            ray = _mainCamera.ScreenPointToRay(_screenCenter);

#if UNITY_EDITOR           
            Debug.DrawRay(_mainCamera.transform.position, _mainCamera.transform.forward * _player.MaxViewDistance, Color.red);
#endif

            if (Physics.Raycast(ray, out Hit, _player.MaxViewDistance, _player.ViewLayers))
            {
                if (Hit.distance < _player.MinViewDistance)
                    _hitPoint = _mainCamera.transform.position + ray.direction * _player.MinViewDistance;
                else
                    _hitPoint = Hit.point;

                if (Hit.distance < _player.InteractionDistance)
                {
                    var objIS = Hit.collider.gameObject.GetComponent<IShowMessage>();
                    if (objIS != null)
                        objIS.ShowMessage();
                    else
                        GameUI.SetMessageBox = string.Empty;

                    if (TryUse)
                    {
                        var objII = Hit.collider.gameObject;
                        var objChek = objII.GetComponent<IInteraction>();
                        if (objChek == null) return;

                        PrepareToInteraction(objII);
                        objChek.Interaction(_gameUI);
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
            foreach (var weapon in Player.Weapons)
            {
                if (!weapon.Equals(enterWeapon)) continue;

                weapon.AvailableForPlayer = active;
                return weapon;
            }
            return default;
        }

        private void PrepareToInteraction(GameObject objII)
        {
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

        #endregion


    }
}