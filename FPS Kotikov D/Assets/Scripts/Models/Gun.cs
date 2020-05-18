using System.Collections;
using UnityEngine;


namespace FPS_Kotikov_D
{
    public sealed class Gun : Weapons
    {


        #region Methods

        public override void Fire(Vector3 hitPoint)
        {
            if (!CanFire) return;
            if (IsReloading) return;
            if (CurrentAmmunition == 0 && Data.CurrentCountClip > 0)
                ReloadClip();

            base.Fire(hitPoint);

            if (CurrentAmmunition > 0)
            {
                var projectorDelay = Vector3.Distance(transform.position, hitPoint) / Data.BulletSpeed;

                var partical = Instantiate(Data.ParticalShoot, _bulletSpawn.position, _bulletSpawn.rotation);
                var particalRB = partical.GetComponent<Rigidbody>();
                particalRB.AddForce(_bulletSpawn.forward * Data.BulletSpeed);
                Destroy(partical, Ammunition.TimeToDestruct);

                CanFire = false;
                StartCoroutine(PlaceBullet(hitPoint, projectorDelay));
                Invoke(nameof(ReadyShoot), Data.RechargeTime);

                CurrentAmmunition--;
                
            }
        }

        private IEnumerator PlaceBullet(Vector3 hitPoint, float projectorDelay)
        {
            yield return new WaitForSeconds(projectorDelay);
            var ammo = Instantiate(Ammunition, hitPoint, _bulletSpawn.rotation);
            ammo.Direction = Vector3.Normalize(hitPoint - transform.position);
            StopCoroutine(nameof(PlaceBullet));
        }

        #endregion


    }
}