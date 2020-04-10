using UnityEngine;


namespace FPS_Kotikov_D
{

    /// <summary>
    /// Base class all objects in the scene
    /// </summary> 
    public abstract class BaseObjectScene : MonoBehaviour
    {


        #region Fields
        [SerializeField, Range(1, 50)] protected int _springJointForceMyltipler = 15;
        [SerializeField, Range(1, 50)] protected int _throwForceMultipler = 15;

        protected int _layer;
        protected Color _color;
        protected Material _material;
        protected Transform _myTransform;
        protected Vector3 _position;
        protected Quaternion _rotation;
        protected Vector3 _scale;
        protected GameObject _instanceObject;
        protected Rigidbody _rigidbody;
        protected string _name;
        protected bool _isVisible;
        protected bool _isRaised = false;


        #endregion


        #region Properties

        public int ThrowForceMultipler
        {
            get => _throwForceMultipler;
            set { _throwForceMultipler = value; }
        }

        public bool IsRaised
        {
            get => _isRaised;
            set { _isRaised = value; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                InstanceObject.name = _name;
            }
        }

        public int Layers
        {
            get { return _layer; }
            set
            {
                _layer = value;
                if (_instanceObject != null)
                {
                    _instanceObject.layer = _layer;
                    AskLayer(GetTransform, _layer);
                }
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                AskColor(GetTransform, _color);
            }
        }

        public Vector3 Position
        {
            get
            {
                if (InstanceObject != null)
                {
                    _position = GetTransform.position;
                }
                return _position;
            }
            set
            {
                _position = value;
                if (InstanceObject != null)
                {
                    GetTransform.position = _position;
                }
            }
        }

        public Vector3 Scale
        {
            get
            {
                if (InstanceObject != null)
                {
                    _scale = GetTransform.localScale;
                }
                return _scale;
            }
            set
            {
                _scale = value;
                if (InstanceObject != null)
                {
                    GetTransform.localScale = _scale;
                }
            }
        }

        public Quaternion Rotation
        {
            get
            {
                if (InstanceObject != null)
                {
                    _rotation = GetTransform.rotation;
                }

                return _rotation;
            }
            set
            {
                _rotation = value;
                if (InstanceObject != null)
                {
                    GetTransform.rotation = _rotation;
                }
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                if (_instanceObject.GetComponent<MeshRenderer>())
                    _instanceObject.GetComponent<MeshRenderer>().enabled = _isVisible;
                if (_instanceObject.GetComponent<SkinnedMeshRenderer>())
                    _instanceObject.GetComponent<SkinnedMeshRenderer>().enabled = _isVisible;
            }
        }

        public Rigidbody Rigidbody
        {
            get { return _rigidbody; }
        }

        public GameObject InstanceObject
        {
            get { return _instanceObject; }
        }

        public Transform GetTransform
        {
            get { return _myTransform; }
        }

        public Material GetMaterial
        {
            get { return _material; }
        }

        #endregion


        #region UnityMethods

        protected virtual void Awake()
        {
            _instanceObject = gameObject;
            _name = _instanceObject.name;
            _rigidbody = _instanceObject.GetComponent<Rigidbody>();
            _myTransform = _instanceObject.transform;
            if (GetComponent<Renderer>())
                _material = GetComponent<Renderer>().material;
        }

        #endregion


        #region Methods

        public void SaveData()
        {
            if (GetComponent<ISerializable>() != null)
                Object.FindObjectOfType<SerializableObjects>().PrefubsForSave.Add(gameObject);
        }

        /// <summary>
        /// Set layer for current object and all his childrens for any level of attachment
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="lvl">Layer</param>
        private void AskLayer(Transform obj, int lvl)
        {
            obj.gameObject.layer = lvl;
            if (obj.childCount > 0)
            {
                foreach (Transform d in obj)
                {
                    AskLayer(d, lvl);
                }
            }
        }

        /// <summary>
        /// Set color for current object and all his childrens for any level of attachment
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="mat">Material</param>
        /// <param name="color">Color</param>
        private void AskColor(Transform obj, Color color)
        {
            if (obj.TryGetComponent<Renderer>(out var renderer))
            {
                foreach (var curMaterial in renderer.materials)
                {
                    curMaterial.color = color;
                }
            }
            if (obj.childCount <= 0) return;
            foreach (Transform d in obj)
            {
                AskColor(d, color);
            }
        }

        protected void ShowName()
        {
            if (_isRaised)
                GameUI.SetMessageBox = string.Empty;
            else
                GameUI.SetMessageBox = gameObject.name;
        }

        protected void RaiseUp()
        {
            if (_isRaised == true) return;
            _isRaised = true;

            var rb = gameObject.GetComponent<Rigidbody>();
            if (rb == null)
                rb = gameObject.AddComponent<Rigidbody>();

            var bc = gameObject.GetComponent<BoxCollider>();
            if (bc == null)
                bc = gameObject.AddComponent<BoxCollider>();

            rb.useGravity = false;
            //rb.interpolation = RigidbodyInterpolation.Extrapolate;
           // rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            Player.Interaction.connectedBody = rb;
            Player.Interaction.connectedAnchor = bc.center;
            Player.Interaction.spring = rb.mass * _springJointForceMyltipler;
        }


        #endregion


    }
}