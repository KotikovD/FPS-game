using UnityEngine;


namespace FPS_Kotikov_D
{
    /// <summary>
    /// Start programm class
    /// </summary>
    public sealed class MainController : MonoBehaviour
    {

        private Controllers _controllers;


        #region UnityMethods


        public void Start()
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
