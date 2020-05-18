using UnityEngine;
using UnityEngine.AI;


namespace FPS_Kotikov_D
{
    public static class Patrol
    {

        private const float MINDISTANCE = 5f;
        private const float MAXDISTANCE = 100f;

        public static bool GenericNewPoint(out Vector3 point, Vector3 agent = default, int area = NavMesh.AllAreas)
        {
            var dis = Random.Range(MINDISTANCE, MAXDISTANCE);
            var randomPoint = Random.insideUnitSphere * dis;

            if (NavMesh.SamplePosition(agent + randomPoint, out var hit, 1f, area))
            {
                point = hit.position;
                return true;
            }
            point = agent;
            return false;
        }
    }
}
