using System;
using UnityEngine;

namespace FPS_Kotikov_D
{
    public sealed class BodyBot : BaseObjectScene, ISetDamage
    {
        public event Action<InfoCollision> OnApplyDamageChange;

        public void SetDamage(InfoCollision info)
        {
            OnApplyDamageChange?.Invoke(info);
        }
    }
}
