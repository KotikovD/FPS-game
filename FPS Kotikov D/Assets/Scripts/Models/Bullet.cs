using UnityEngine;


namespace FPS_Kotikov_D
{

    public class Bullet : Ammunition
    {

        [SerializeField] private int _maxRicochetTimes = 2;
        [SerializeField] private float _damageRicochetDivider = 2.0f;
        [SerializeField] private float _lifeTime = 1f;

        private int _ricochetCounter = 0;

        private void OnCollisionEnter(Collision collision)
        {
//#if UNITY_EDITOR
//            Debug.Log("Current bullet damage = " + _currentDamage);
//#endif

            var tempObj = collision.gameObject.GetComponent<ISetDamage>();
            if (tempObj != null)
            {
                tempObj.SetDamage(new InfoCollision(_currentDamage, collision.contacts[0],
                    collision.transform, Rigidbody.velocity));
                    DestroyAmmunition(_lifeTime);
            }
        }


    }
}