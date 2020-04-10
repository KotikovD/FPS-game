using UnityEngine;
using UnityEngine.AI;

namespace FPS_Kotikov_D
{
    public static class Patrol
    {
        public static Vector3 GenericPoint(Transform agent)
        {
            //todo перемещение по точкам
            Vector3 result;

            var dis = Random.Range(5, 25);
            var randomPoint = Random.insideUnitSphere * dis;

            //if (NavMesh.SamplePosition(randomPoint, out var hit, dis, NavMesh.AllAreas))
            //{


            //}

                NavMesh.SamplePosition(agent.position + randomPoint,
                out var hit, dis, NavMesh.AllAreas);
            result = hit.position;

            return result;
        }
    }
}
