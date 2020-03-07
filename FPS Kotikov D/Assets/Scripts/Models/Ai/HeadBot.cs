using System;
using UnityEngine;

namespace FPS_Kotikov_D
{
    public sealed class HeadBot : BaseObjectScene, ISetDamage
    {

        [SerializeField] private float _damageMultipler = 10;

        public event Action<InfoCollision> OnApplyDamageChange;

        public void SetDamage(InfoCollision info)
        {
            OnApplyDamageChange?.Invoke(new InfoCollision(info.Damage * _damageMultipler,
                info.Contact, info.ObjCollision, info.Dir));
        }
    }
}
