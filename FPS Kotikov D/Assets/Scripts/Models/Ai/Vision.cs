using UnityEngine;


namespace FPS_Kotikov_D
{
    [System.Serializable]
    public sealed class Vision
    {


        #region Fileds

        public float ActiveDistance = 10.0f;
        public float ActiveAngle = 35.0f;

        #endregion


        #region Methods

        public bool CanBotSeePlayer(Transform player, Transform target)
        {
            return CheckVisionDistance(player, target)
                && CheckSeeAngle(player, target)
                && CheckObstacles(player, target);
        }

        public bool BotLostPlayer(Transform player, Transform target)
        {
            return !CheckVisionDistance(player, target)
                || !CheckObstacles(player, target);
        }

        private bool CheckObstacles(Transform player, Transform target)
        {
            return Physics.Linecast(player.position, target.position);
        }

        private bool CheckSeeAngle(Transform player, Transform target)
        {
            var angle = Vector3.Angle(player.forward, target.position - player.position);
            return angle < ActiveAngle;
        }

        private bool CheckVisionDistance(Transform player, Transform target)
        {
            var currentDistance = Vector3.Distance(player.position, target.position);
            return currentDistance < ActiveDistance;
        }

        #endregion


    }
}
