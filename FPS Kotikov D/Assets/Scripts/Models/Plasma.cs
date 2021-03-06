﻿using UnityEngine;


namespace FPS_Kotikov_D
{

    public class Plasma : Ammunition
    {

        private void OnCollisionEnter(Collision collision)
        {
#if UNITY_EDITOR
            Debug.Log("Current bullet damage = " + _currentDamage);
#endif
            var tempObj = collision.gameObject.GetComponent<ISetDamage>();

            if (tempObj != null)
            {
                tempObj.SetDamage(new InfoCollision(_currentDamage, collision.contacts[0],
                    collision.transform, Rigidbody.velocity));
            }

            DestroyAmmunition();
        }


    }
}