using UnityEngine;


namespace FPS_Kotikov_D
{

    public class Bullet : Ammunition
    {

        private void OnCollisionEnter(Collision collision)
        {

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