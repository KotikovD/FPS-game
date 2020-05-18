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
            Debug.Log(info.Damage * _damageMultipler + "head damage");
            OnApplyDamageChange?.Invoke(info);
        }
    }
}
