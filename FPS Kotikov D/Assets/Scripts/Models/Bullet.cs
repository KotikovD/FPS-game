using UnityEngine;


namespace FPS_Kotikov_D
{

    public class Bullet : Ammunition
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


            // Rigidbody.AddForce(Direction * _addForcePower);

            var rbObj = collision.gameObject.GetComponent<Rigidbody>();
            if (rbObj != null)
            {
                rbObj.AddForceAtPosition(Direction * _addForcePower, collision.contacts[0].point, ForceMode.Impulse);
                // rbObj.AddForce(Direction * _addForcePower);
            }

            DestroyAmmunition(_timeToDestruct);
        }


    }
}