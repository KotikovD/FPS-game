using UnityEngine;


namespace FPS_Kotikov_D
{

    public class Bullet : Ammunition
    {

        [SerializeField] private int _maxRicochetTimes = 2;
        [SerializeField] private float _damageRicochetDivider = 2.0f;

        private int _ricochetCounter = 0;

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

                if (collision.gameObject.GetComponent<Wall>() && _ricochetCounter < _maxRicochetTimes)
                {
#if UNITY_EDITOR
                    Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.red, 100);
#endif
                    Rigidbody.velocity = Vector3.Reflect(Rigidbody.velocity.normalized, collision.contacts[0].normal) * Rigidbody.velocity.magnitude;
                    _currentDamage /= _damageRicochetDivider;
                    _ricochetCounter++;
                }
                else
                    DestroyAmmunition();
            }


        }


    }
}