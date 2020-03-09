using UnityEngine;

namespace FPS_Kotikov_D
{
    [System.Serializable]
    public sealed class Vision
    {
        public float ActiveDistance = 10.0f;
        public float ActiveAngle = 35.0f;
        public float LostPlayerDistance = 5.0f;

        public bool VisionPlayer(Transform player, Transform target)
        {
            return Distance(player, target, ActiveDistance) && Angle(player, target) && !CheckBlocked(player, target);
        }

        public bool LostPlayer(Transform player, Transform target)
        {
            if (CheckBlocked(player, target))
                return true;
            if (Distance(player, target, LostPlayerDistance) && CheckBlocked(player, target))
                return true;
            return false;
        }

        private bool CheckBlocked(Transform player, Transform target)
        {
            if (!Physics.Linecast(player.position, target.position, out var hit))
                return true;
            return hit.transform != target;
        }

        private bool Angle(Transform player, Transform target)
        {
            var angle = Vector3.Angle(player.forward, target.position - player.position);
            return angle <= ActiveAngle;
        }

        private bool Distance(Transform player, Transform target, float distance)
        {
            var dist = Vector3.Distance(player.position, target.position); //todo оптимизация
            return dist <= distance;
        }

    }
}
