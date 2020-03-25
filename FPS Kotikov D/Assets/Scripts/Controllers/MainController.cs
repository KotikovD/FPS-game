using UnityEngine;
using FPS_Kotikov_D.Data;

namespace FPS_Kotikov_D
{
    /// <summary>
    /// Start programm class
    /// </summary>
    public sealed class MainController : MonoBehaviour
    {

        private Controllers _controllers;


        public static MainController Instance { get; private set; }
        public SaveDataRepository SaveDataRepository { get; private set; }



        #region UnityMethods

        private void Awake()
        {
            Instance = this;

            //  MainCamera = Camera.main;
            
            SaveDataRepository = new SaveDataRepository();


        }

        private void Start()
        {
            _controllers = new Controllers();
            _controllers.Initialization();
        }

        private void Update()
        {
            for (var i = 0; i < _controllers.Length; i++)
            {
                _controllers[i].Execute();
            }
        }

        #endregion


    }
}
