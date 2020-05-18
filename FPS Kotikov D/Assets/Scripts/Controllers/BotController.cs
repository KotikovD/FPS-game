using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace FPS_Kotikov_D
{
    public sealed class BotController : BaseController, IExecute, IInitialization
    {


        #region Fields

        private readonly int _countBot = 10;
        private readonly HashSet<Bot> _getBotList = new HashSet<Bot>();

        #endregion


        #region Methods

        public void Initialization()
        {
            for (var index = 0; index < _countBot;)
            {
                //todo разных противников
                var Bot = ServiceLocatorMonoBehaviour.GetService<Reference>().Bot;

                if (!Patrol.GenericNewPoint(out var point))
                    continue;

                var tempBot = Object.Instantiate(Bot,point,Quaternion.identity);
                
                tempBot.name = Bot.name;
                tempBot.Agent.avoidancePriority = index;
                tempBot.Target = ServiceLocatorMonoBehaviour.GetService<CharacterController>().transform;
                
                AddBotToList(tempBot);
                index++;
            }
        }

        //public bool GenericPoint(Transform agent, out Vector3 point)
        //{

        //    var dis = Random.Range(25, 100);
        //    var randomPoint = Random.insideUnitSphere * dis;

        //    if (NavMesh.SamplePosition(agent.position + randomPoint, out var hit, 1f, NavMesh.AllAreas))
        //    {
        //        point = hit.position;
        //        return true;
        //    }
        //    point = agent.position;
        //    return false;
        //}

        private void AddBotToList(Bot bot)
        {
            if (!_getBotList.Contains(bot))
            {
                _getBotList.Add(bot);
                bot.OnDieChange += RemoveBotToList;
            }
        }

        private void RemoveBotToList(Bot bot)
        {
            if (!_getBotList.Contains(bot)) return;

            bot.OnDieChange -= RemoveBotToList;
            _getBotList.Remove(bot);
        }

        public void Execute()
        {
            if (!IsActive) return;

            for (var i = 0; i < _getBotList.Count; i++)
            {
                var bot = _getBotList.ElementAt(i);
                bot.Tick();
            }
        }

        #endregion


    }
}
