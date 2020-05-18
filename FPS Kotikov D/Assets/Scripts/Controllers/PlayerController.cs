using DG.Tweening;
using UnityEngine;


namespace FPS_Kotikov_D.Controller
{
    public class PlayerController : BaseController, IExecute, IInitialization
    {

        #region Fields

        public static RaycastHit Hit;

        public bool TryUse = false;

        private GameUI _gameUI;
        private Camera _mainCamera;
        private Vector2 _screenCenter;
        private Ray ray;
        private CharacterController _charController;
        

        #endregion


        #region Properties

        public static float MoveMagnitude { get; private set; }

        public Vector3 HitPoint { get; private set; }
        public Player Player { get; private set; }

        #endregion


        #region Methods

        public void Initialization()
        {
            Player = Object.FindObjectOfType<Player>();
            PlayerPresenter.Player = Player;
            _charController = Player.GetComponent<CharacterController>();
            Player.AddWeapons();
            _gameUI = Object.FindObjectOfType<GameUI>();
            _mainCamera = Camera.main;
            _screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            On();
        }

        public void Execute()
        {
            if (!IsActive) return;
            if (!_mainCamera) return;
            MoveMagnitude = _charController.velocity.magnitude;
            _gameUI.PlayerHpText = FPS_Kotikov_D.Player.CurrentHp;
            ray = _mainCamera.ScreenPointToRay(_screenCenter);

#if UNITY_EDITOR           
            Debug.DrawRay(_mainCamera.transform.position, _mainCamera.transform.forward * Player.MaxViewDistance, Color.red);
#endif

            if (Physics.Raycast(ray, out Hit, Player.MaxViewDistance, Player.ViewLayers))
            {
                if (Hit.distance < Player.MinViewDistance)
                    HitPoint = _mainCamera.transform.position + ray.direction * Player.MinViewDistance;
                else
                    HitPoint = Hit.point;

                if (Hit.distance < Player.InteractionDistance)
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
                HitPoint = _mainCamera.transform.position + ray.direction * Player.MaxViewDistance;
            }
        }

        /// <summary>
        /// Set avalibale for player weapons pool
        /// </summary>
        public Weapons SwitchActiveWeapon(Weapons enterWeapon, bool active)
        {
            foreach (var weapon in FPS_Kotikov_D.Player.Weapons)
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

            Player.Interaction.MySpringJoint.connectedBody = rb;
            Player.Interaction.MySpringJoint.connectedAnchor = bc.center;
            Player.Interaction.MySpringJoint.spring = rb.mass * Player.Interaction.SpringJointForceMyltipler;
        }

       
        #endregion


    }
}